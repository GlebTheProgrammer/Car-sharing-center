// Function for getting data in the current month
function GetDaysInCurrentMonth() {
    const date = new Date();

    return new Date(
        date.getFullYear(),
        date.getMonth() + 1,
        0
    ).getDate();
}

// Function for calculating time period after user selects dateTimePicker time
function CalculateVehicleUsagePeriodAndSetUpAllInputs(datetimesStartsStr, datetimesEndsStr, vehicleHourlyTariff, vehicleDailyTariff) {
    var arrayStarts = datetimesStartsStr.split(' ');
    var arrayEnds = datetimesEndsStr.split(' ');

    var dayStarts = arrayStarts[2];
    var timeStarts = arrayStarts[4];

    var dayEnds = arrayEnds[2];
    var timeEnds = arrayEnds[4];

    var timeStartsInt = parseInt(timeStarts.slice(0, 2));
    var timeEndsInt = parseInt(timeEnds.slice(0, 2));
    var dayStartsInt = parseInt(dayStarts);
    var dayEndsInt = parseInt(dayEnds);


    var totalHourlyPrice;
    var totalDailyPrice;
    var totalPrice;

    if (dayEndsInt > dayStartsInt && timeStartsInt > timeEndsInt) // 28 23:00 -> 30 1:00
    {
        totalDailyPrice = (dayEndsInt - dayStartsInt - 1) * vehicleDailyTariff;
        totalHourlyPrice = (24 - timeStartsInt + timeEndsInt) * vehicleHourlyTariff;
    }
    else {

        if (dayEndsInt < dayStartsInt && timeStartsInt > timeEndsInt) // 28 23:00 -> 1 1:00
        {
            totalDailyPrice = (GetDaysInCurrentMonth() - dayStartsInt - 1 + dayEndsInt) * vehicleDailyTariff;
            totalHourlyPrice = (24 - timeStartsInt + timeEndsInt) * vehicleHourlyTariff;
        }
        else {
            if (dayEndsInt < dayStartsInt && timeStartsInt <= timeEndsInt) // 28 1:00 -> 1 23:00
            {
                totalDailyPrice = (GetDaysInCurrentMonth() - dayStartsInt + dayEndsInt) * vehicleDailyTariff;
                totalHourlyPrice = (timeEndsInt - timeStartsInt) * vehicleHourlyTariff;
            }
            else { // 28 1:00 -> 30 23:00
                totalDailyPrice = (dayEndsInt - dayStartsInt) * vehicleDailyTariff;
                totalHourlyPrice = (timeEndsInt - timeStartsInt) * vehicleHourlyTariff;
            }
        }
    }

    totalPrice = totalHourlyPrice + totalDailyPrice;

    document.getElementById("totalDailyPrice").textContent = "$" + totalDailyPrice + ".00";
    document.getElementById("totalHourlyPrice").textContent = "$" + totalHourlyPrice + ".00";
    document.getElementById("totalPrice").textContent = "$" + (totalPrice) + ".00";

    document.getElementById("totalPriceInput").value = totalPrice;

    document.getElementById("rentalStartsDateTimeLocalStrInput").value = datetimesStartsStr;
    document.getElementById("rentalEndsDateTimeLocalStrInput").value = datetimesEndsStr;

}