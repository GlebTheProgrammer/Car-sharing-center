// Function for showing the alert when user click on Cancel Changes button
function ShowCancelChangesConfirmationAlert() {
    Swal.fire({
        title: 'Are you sure?',
        text: "All new information will be removed!",
        icon: 'question',
        showCancelButton: true,
        confirmButtonColor: '#4FA64F',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.reload(true);
        }
    });
}

// Function for working with change password
function SubmitChangePasswordForm(form) {
    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
    }
    else {
        if (document.getElementById("newPassword").value === document.getElementById("confirmNewPassword").value) {
            $("#changePasswordModal").modal('hide');
/*            window.location.reload(true);*/
            ShowSuccessfulMessageAfterUserHasChangedPassword();
            /*document.getElementById("ChangePasswordForm").submit();*/
        }
        else {
            event.preventDefault();
            document.getElementById("confirmNewPassword").setAttribute("class", "form-control is-invalid");
        }
    }
    form.classList.add('was-validated');
}

// Function for setting input as the specific image src value
function SetUpImageDependsOnTheModal(azureStoragePath) {
    var imageInput = document.getElementById("ModelUserImage");
    var userImage = document.getElementById("UserImage");

    var image1 = document.getElementById("Image1").src;
    var image2 = document.getElementById("Image2").src;
    var image3 = document.getElementById("Image3").src;
    var image4 = document.getElementById("Image4").src;
    var image5 = document.getElementById("Image5").src;
    var image6 = document.getElementById("Image6").src;
    var image7 = document.getElementById("Image7").src;

    var image1Radio = document.getElementById("flexRadioPicture1");
    var image2Radio = document.getElementById("flexRadioPicture2");
    var image3Radio = document.getElementById("flexRadioPicture3");
    var image4Radio = document.getElementById("flexRadioPicture4");
    var image5Radio = document.getElementById("flexRadioPicture5");
    var image6Radio = document.getElementById("flexRadioPicture6");
    var image7Radio = document.getElementById("flexRadioPicture7");

    if (image1Radio.checked) {
        imageInput.value = image1.toString().replace(azureStoragePath, "");
        userImage.setAttribute("src", image1);
    }
    if (image2Radio.checked) {
        imageInput.value = image2.toString().replace(azureStoragePath, "");
        userImage.setAttribute("src", image2);
    }
    if (image3Radio.checked) {
        imageInput.value = image3.toString().replace(azureStoragePath, "");
        userImage.setAttribute("src", image3);
    }
    if (image4Radio.checked) {
        imageInput.value = image4.toString().replace(azureStoragePath, "");
        userImage.setAttribute("src", image4);
    }
    if (image5Radio.checked) {
        imageInput.value = image5.toString().replace(azureStoragePath, "");
        userImage.setAttribute("src", image5);
    }
    if (image6Radio.checked) {
        imageInput.value = image6.toString().replace(azureStoragePath, "");
        userImage.setAttribute("src", image6);
    }
    if (image7Radio.checked) {
        imageInput.value = image7.toString().replace(azureStoragePath, "");
        userImage.setAttribute("src", image7);
    }
}

// Function for working wich checkBox element
function ChangeIsAcceptingNewsModelState(state) {

    var temp = document.getElementById("ModelIsAcceptedNewsSharing");

    if (state === 'True') {

        temp.setAttribute("checked", "");
    }
    else {

        temp.removeAttribute("checked");
    }
}

// Function for showing input field
function ShowInputField(target) {

    var text = document.getElementById(target + "Text");
    var changeBtn = document.getElementById("Change" + target + "Btn");
    var input = document.getElementById(target + "Input");
    var saveBtn = document.getElementById("Save" + target + "Btn");

    text.setAttribute("class", "visually-hidden");
    changeBtn.setAttribute("class", "visually-hidden");

    input.setAttribute("class", "form-control");
    input.value = text.textContent;
    saveBtn.setAttribute("class", "btn btn-outline-success");
}

// Function for hiding input field
function HideInputField(target, form) {
    if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
    } else {
        var text = document.getElementById(target + "Text");
        var changeBtn = document.getElementById("Change" + target + "Btn");
        var input = document.getElementById(target + "Input");
        var saveBtn = document.getElementById("Save" + target + "Btn");

        text.textContent = input.value;
        SetModelValue(target);

        input.setAttribute("class", "visually-hidden");
        saveBtn.setAttribute("class", "visually-hidden");

        text.setAttribute("class", "text-muted mb-0");
        changeBtn.setAttribute("class", "btn btn-outline-warning");

        event.preventDefault();
    }
    form.classList.add('was-validated');
}

// Function for setting form input value
function SetModelValue(valueName) {

    var input = document.getElementById(valueName + "Input");
    var modelAttribute = document.getElementById("Model" + valueName);

    modelAttribute.value = input.value;
}

// Function for hiding error span values
function HideErrorSpan(componentId) {
    var component = document.getElementById(componentId);
    component.setAttribute("class", "visually-hidden");
}