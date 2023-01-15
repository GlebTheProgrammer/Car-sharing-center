# SamSolutions Project - Car Sharing Center (На момент 15 января 2023 г.)

Необходимо разработать приложение-сервис каршеринговой службы с отображением доступных автомобилей на карте. По содержанию - всё, что можно сопоставить с сущностью `Vehicles`: Автомобили, мотоциклы, любые транспортные средства и тд. **Перед прочтением документации по страницам приложения, настоятельно рекомендую прочесть техническую документацию, которая расположена в конце страницы. Она поможет лучше разобраться, каким образом происходит работа приложения и что вообще происходит за кулисами.** 

p.s. Так как всё очень быстро и часто обновляется, техническая документация может являться устаревшей и какие-то критерии могут не соответствовать действительности. 

### История коммитов с пояснением

- 12 октября 2022 - Начало проекта.
- 13 октября 2022 - Начало работы с главной страницей и страницей каталога. Составление структуры моделей.
- 15 октября 2022 - Добавление GoogleMaps API. Начало работы со страницами регистрации и входа.
- 18 октября 2022 - Рефакторинг + работа со страницей добавления своего автомобиля в каталог.
- 24 октября 2022 - Рефакторинг. Сериализация и десериализация. Доработка страницы и логики добавления своего автомобиля.
- 25 октября 2022 - Работа со страницей каталога. Добавление `pager`-а на страницу.
- 26-28 октября 2022 - Работа с аутентификацией пользователя на основе сессий и `httpContextAccessor`.
- 31 октября 2022 - Отказ от работы с сессиями в связи с большим количеством контроллеров. Переход с сессий на объект хранения статуса `UserStatusProvider`. Добавление `JS` сообщений с использованием сервиса `SweetAlert2`.
- 1 ноября 2022 - Тестирование сервиса, добавление информации на страницу. Доработка страницы добавления своего автомобиля.
- 2 ноября 2022 - Добавление страницы с информацией об автомобиле из каталога. Добавление логики контроля доступа для неавторизованного пользователя.
- 3-4 ноября 2022 - Работа с `ReadMe`.
- 10-16 ноября 2022 - Продумывание идеи оформления заказов. Реализация страницы пользователя с частичным функционалом (неполным).
- 17 ноября 2022 - Рефакторинг.
- 19 ноября 2022 - Работа с кабинетом пользователя. Полностью реализовал функционал редактирования информации пользователя. Начал делать страницу редактирования автомобилей.
- 20 ноября 2022 - Рефакторинг. Доделал страницу редактирования информации об автомобиле.
- 21 ноября 2022 - Подключение в тестовом режиме платёжного сервиса `Stripe` к проекту. Добавление функционала выбора времени и даты на страницу с оформлением заказа.
- 23 ноября 2022 - Доделал часть с оформлением заказа. Поменял расположение `JS` кода (разнёс по файлам).
- 26 ноября 2022 - Доделал часть с отслеживанием просроченных заказов.
- 28 ноября 2022 - Добавление сообщений об успешных действиях пользователя. Добавил в каталоге возможность видеть всем пользователям время с последнего заказа того или иного автомобиля. Добавил возможность на странице с информацией об автомобиле из каталога посмотерть так называемое `Brief Description` аккаунта разместившего автомобиль пользователя. Начал делать часть с выставлением рейтинга для автомобилей.
- 30 ноября 2022 - Закончил делать часть с выставлением рейтинга для автомобилей (когда пользователь завершает активный заказ на автомобиль из своего личного кабинета). Небольшой рефакторинг.
- 2 декабря 2022 - Переход на `.NET 7`. Работа над поисковой системой страницы каталога.
- 3 декабря 2022 - Замена огромного количества разных репозиториев одним `RepositoryProvider` и сведение всей функциональности под него.
- 5 декабря 2022 - Добавление `JWToken Authentication` на контроллеры `Web`-приложения.
- 8 декабря 2022 - Рефакторинг части с контроллерами и логики взаимодействия с ними.
- 11 декабря 2022 - Работа с `JWToken` и настройка отображения страницы с ошибкой на стороне клиента.
- 16 декабря 2022 - Рефакторинг с целью перехода от хранения информации в `JSON`-файлах к хранению в `MongoDB Local`.
- 17 декабря 2022 - Настройка кластера `MongoDB Atlas` с целью дальнейшей работы с облачным сервисом.
- 19 декабря 2022 - Рефакторинг. Работа над `UI`. Изменение маркера `Google Maps API` с красного на автомобиль с названием и изображением.

**Официальное начало стажировки**

- 20 декабря 2022 - Переход от одного `Web`-приложения к полноценной `Clean Architecture`.
- 21 декабря 2022 - Работа с `Domain Layer` и настройка первых моделей. Модели создаются в стиле `Rich Domain Models` заместо `Anemic Domain Models`.
- 22 декабря 2022 - Работа с `Application` и `Infrastructure Layers`, а также настройка первых взаимодействий с сервисом и настройка `DI`.
- 23 декабря 2022 - Связывание ранее созданных слоёв вместе с `PublicAPI`.
- 24 декабря 2022 - Настройка первых `endpoints` и их ручное тестирование.
- 25 декабря 2022 - Начало работы над `Errors Handling` и его обработки.
- 26 декабря 2022 - Добавление `Errors Handling` в часть модели `Vehicle`.
- 27 декабря 2022 - Создание контроллера пользователя и его тестирование на `endpoints`.
- 28 декабря 2022 - Добавление функциональности `Azure Key Vault` для получения секретных данных с `Azure`. Внедрение битовых флагов как одного из вариантов хранения информации об автомобилях.
- 30 декабря 2022 - Работа с критерими и ролями, а также добавление новых критериев валидации и привязывание к ним возможных ошибок.
- 1 января 2023 - Добавление новых `endpoints` в `CustomerController` и их ручное тестирование.
- 2 января 2023 - Добавление новых `endpoints` в `VehicleController` и их ручное тестирование.
- 4 января 2023 - Редактирование моделей и сервисов.
- 5 января 2023 - Добавление `IHttpClientFactory` для создания клиентов для отправки requests в сторону `PublicAPI`. Настройка `Polly` для недетерминированной отправке сообщений с возможностью их ожидания и повторной отправки в случае критических ситуаций.
- 7 января 2023 - Настройка межпрограммной сериализации и десериализации (маршалинг и демаршалинг), а также сведение сообщений с ответом с сервера под отдельные сущности.
- 8 января 2023 - Связывание `PublicApi` вместе с `Web`-частью и проверка endpoints и межпрограммной сериализации и десериализации.
- 10 января 2023 - Работа над страницей входа и сохранение полученного `JWToken` с сервера на стороне клиента при успешном входе в аккаунт.
- 11 января 2023 - Работа на `UI` страниц регистрации, авторизации и добавления нового автомобиля.
- 12 января 2023 - Добавление `errors handling` на клиентской части при добавлении нового автомобиля.
- 13 января 2023 - Ревакторинг `html`-страниц и некоторых `endpoints`.
- 14 января 2023 - Переход к более новой версии `Bootstrap 5+` и переделывание логики страниц регистрации и авторизации.
- 15 января 2023 - Работа со страницей добавления нового автомобиля и исправление части с валидацией.

### О том, как я вижу этот проект

Этот проект представляет из себя полноценное `Web`-приложение, работающее на одном уровне с `PublicAPI`, написанным в стиле `Clean Architecture`, позволяющее любому человеку, имеющему валидную карту для оплаты аренды и водительское удостоверение получить возможность арендовать на время машину другого пользователя, который также зарегистрировался, но не с целью аренды чужого автомобиля, а с целью предоставления своих автомобилей для пользования другим пользователям. Таким образом, выгоду могут получить как и разработчики приложения, получая процент с осуществляемых пользователями сделок, так и непосредственно сами пользователи, арендующие либо предоставляющие в аренду свои автомобили.

## Что реализовано на данный момент

- Регистрация нового пользователя
- Авторизация
- Главная страница с информацией о сервисе
- Каталог с информацией о доступных автомобилях
- Страница добавления своего личного автомобиля авторизованным пользователем

## Какие сервисы и вспомогательные библиотеки хочется отметить

- Всплывающие уведомления (`SweetAlert2`)
- Отображение автомобилей на карте (`Google Maps API`)
- Оплата аренды автомобилей (`Stripe`)
- Errors handling (`ErrorOr NuGet Package`)
- Авторизация (`JWT Bearer NuGet Package`)
- Получение секретных значений (`Azure Key Vault`)
- Хостинг (`Azure VM`)

и другие...

**Часть ниже также реализована в первичном виде, однако не доступна на момент написания этого ReadMe. Релиз нижней части планируется в ближайшее время...**

- Страница просмотра информации об автомобиле из каталога для авторизованного пользователя
- Страница - личный кабинет клиента для авторизованных пользователей
- Страница с редактированием информации аккаунта для авторизованных пользователей
- Страница с редактированием информации автомобиля клиента для авторизованного пользователя
- Своеобразная страница - pop up для выбора промежутка времени и оформления заказа для авторизованного пользователя
- Интегрированная страница с платёжным сервисом `Stripe` для платежей авторизованным пользователем

## Какие планы

- Выпустить все фичи, описанные выше, в ближайшее время
- Добавление админской части на Angular 
- Работа над UI приложением

# О технической состаляющей проекта и о его функциях + GUI

В данном разделе я попытался максимально развёрнуто объяснить, как что работает, почему была выбрана именно такая стратегия, с какими трудностями мне пришлось столкнуться и как я их решал (а может быть и не решал (-_-) )

Весь проект был написан с нуля, при помощи bootstrap для оформления визуальной составляющей, с использованием функциональных возможностей Razor Pages, ну и C# .NET Core для написания backend составляющей. В качестве модели взаимодействия мной был сделан выбор в сторону MVC.

### Главная страница и её составляющие

![1](https://user-images.githubusercontent.com/55713244/199783450-c1b8ad2a-3062-4411-901b-f7e5888c1f97.jpg)

Как только неавторизованный пользователь запускает приложение, он сразу же видит главный экран с информацией о сервисе. На данной странице находится вся необходимая информация для ознакомления нового пользователя со всеми возможностями сервиса. Сверху можно видеть элемент `navbar`, который всё время находится на любой из страниц и не пропадает из поля видения, предназначенный для осуществления навигации пользователя по страницам приложения. `Navbar` предоставляет следующие возможности:

1) Перейти на главную страницу с информацией о сервисе
2) Перейти в каталог с доступными автомобилями
3) Перейти на страницу регистрации (для неавторизованного пользователя)
4) Перейти на страницу авторизации (для неавторизованного пользователя)
5) Выйти из аккаунта (для авторизованного пользоавтеля)
6) Перейти на страницу с публикованием своего собственного автомобиля (для авторизованного пользователя)

![2](https://user-images.githubusercontent.com/55713244/199786125-db5a6af0-501a-4597-b247-93a2e42d153f.jpg)

Также, на странице присутствует карта от google, которая показывает расположение абсолютно всех автомобилей в каталоге (на карте появляются маркеры, по котороым можно отследить текущее местоположение транспортных средств). В качестве ограничения, я выбрал город Минск, то есть карта расположена таким образом, чтоб охватить всю область города. Если автомобиль, скажем, будет находиться где-то на улицах Бреста, то на карте мы его не увидим (либо же нужно будет менять масштаб карты). В качестве доступных локаций для размещения автомобилей, я решил ограничиться Беларусью, то есть автомобиль, размещаемый авторизованным пользователем, обязательно должен находиться на территории РБ.

Касательно самой карты, она была подключена в приложение при помощи специального ключа, который работает без каких либо ограничений для поиска местоположения по известным Latitude и Longitude. Однако для получения Latitude и Longitude из строки текста с адресом, потребовалось больше действий (о них будет рассказано ниже).

На любой странице, в самом низу, будет расположена информация о разработчиках приложения и ссылка на соответствующие соц. сети.

### Страница с каталогом и её составляющие

![3](https://user-images.githubusercontent.com/55713244/199789338-78214c30-108e-4292-a29e-61d0772c0b46.jpg)

На странице с каталогом пользователям будет предоставлена возможность просмотреть всю доступную краткую информацию об автомобилях. На странице есть возможность:

- Настроить количество отображаемых автомобилей (3-6-9-12-15)
- Поиск автомобилей по странице (Будет добавлен позже)
- Счётчик отображаемых и доступных автомобилей в левом нижнем углу экрана
- Саморасширяющийся Navbar для перемещения по каталогу в правом нижнем углу экрана

Касательно информации об автомобилях, в каталоге представлены:

- Наименования автомобилей
- Их краткое описание
- Цеза за час и день пользования
- Время с прошлого оформления заказа на этот автомобиль (будет добавлено позже)
- На информацинной карточке присутствует кнопка, при нажатии на которую авторизованный пользователь будет перенаправлен на страницу с более детальной информацией о выбранном транспортном средстве, с которой также сможет оформить заказ на пользование

Таким образом, каталог позволяет рассмотреть ассортимент представленных транспортных средств и выбрать какой-либо экземпляр на свой вкус и цвет 

![4](https://user-images.githubusercontent.com/55713244/199791049-e70e478f-6949-4e92-80e2-d8b96d9c3e35.jpg)

Как только пользователь нажимает кнопку `Sign In`, он немедленно перенаправляется на страницу авторизации.

### Страница авторизации и её составляющие

![5](https://user-images.githubusercontent.com/55713244/199791559-9b7201e7-8cf7-48e1-8a67-3339a127c700.jpg)

На странице авторизации неавторизованному пользователю потребуется ввести свой email и password для успешной авторизации. При ошибке в заполнении поля - будет отображаться сообщение об ошибке при заполнении:

![10](https://user-images.githubusercontent.com/55713244/200046781-c5c5445b-6c5f-45ee-a9cf-1d95c33a065e.jpg)

Если все поля заполнены правильно, но авторизация не прошла - пользователю будет представлено pop up сообщение о неудачной авторизации:

![11](https://user-images.githubusercontent.com/55713244/200047089-da077b56-58c9-481d-8c6a-e2015b4123f3.jpg)

При успешной авторизации, уже авторизованный пользователь, будет перенаправлен на основную страницу, на которой появится сообщение об успешной авторизации, которое автоматически пропадает через 3 секунды после своего появления:

![12](https://user-images.githubusercontent.com/55713244/200047530-64435663-3d16-4208-b309-fbeecaa95709.jpg)

Если же у пользователя нет своего аккаунта, внизу страницы авторизации можно наблюдать сообщение-ссылку, нажатие на которую перенаправит его на страницу с регистрацией.

### Страница регистрации и её составляющие

![6](https://user-images.githubusercontent.com/55713244/199792816-72f1ef25-efec-4459-947b-e58df142dfb7.jpg)

На странице с регистрацией нового пользователя, необходимо будет ввести ряд данных, чтобы программа внесла клиента в базу и регистрация была пройдена успешно. Вся страница наполнена валидацией, и если что-то не пройдёт - пользователь будет уведомлён о том, в каком поле произошла ошибка (такой же приинцип, как и со сраницей авторизации). После успешной регистрации, пользователь будет перенаправлен на страницу с регистрацией, на которой появится сообщение об успешной регистрации:

![13](https://user-images.githubusercontent.com/55713244/200048116-4399e279-19fc-4443-b1d3-cc75fe5af69a.jpg)

Как только пользователь успешно войдёт в свой только что созданный аккаунт, он получит возможность не только просматривать более детальную информацию об автомобилях в каталоге, но и добавлять свои собственные автомобили.

### Страница для публикации своего личного автомобиля 

![7](https://user-images.githubusercontent.com/55713244/199794194-dcceaf86-0003-4bd8-950d-d635b1eac04d.jpg)

На данной странице пользователь должен будет предоставить соответствующую информацию о своём автомобиле, а также установить тариф, по которому будет производиться аренда автомобиля. На странице также присутствует валидация (аналогично странице авторизации и регистрации). Как только пользователь успешно поделится своей машиной, она сразу же появится в каталоге. (Так как пользователь добавил свой автомобиль, логично, что показывать его в каталоге этому же пользователю будет не совсем логично и правильно. Этот нюанс будет устранён в будущем, когда необходимо будет составлять каталог не только согласно пользователю, но и согласно ныне арендованным и не арендованным автомобилям, а также фильтрам).

### Страница с информацией об автомобиле и её составляющие

![8](https://user-images.githubusercontent.com/55713244/199794876-044fbc96-eae7-4562-80ba-b28ca263906b.jpg)

Как только авторизованный пользователь нажимает кнопку `Information` на карточке транспортного средства из каталога, у него открывается соответствующая страница с выбранным автомобилем и более детальной информацией о экземпляре. С этой страницы, вскоре, можно будет оформлять заказы, которые будут фиксироваться в личном кабинете пользователя.

### Личный кабинет пользователя и её составляющие

Мной была добавлена кнопка перехода в личный кабинет пользователя после авторизации, которая позволяет пользователю отслеживать свои заказы и добавленные им автомобили (Пока что активны только кнопки `Publish` и `Hide` для взаимодействия с автомобилями).

![1](https://user-images.githubusercontent.com/55713244/202200263-300217ee-03d6-469e-81df-a53bad5dd8c1.jpg)
![2](https://user-images.githubusercontent.com/55713244/202200295-4f29c858-31b6-4898-bfe7-c1a4749def3c.jpg)

На данной странице, пользователь сможет не только просматривать и изменять информацию своего акканута, но и управлять своими автомобилями и своими заказами. После данных пользователя, можно увидеть следующие счётчики:

- Vehicles added - Сколько автомобилей было добавлено пользователем (Фактически, количество автомобилей в личном кабинете)
- Active vehicles - Сколько автомобилей опубликовано и доступно для аренды другими пользователями
- Active orders - Сколько заказов закреплено за пользователем (Фактически, количество активных заказов аренды автомобиля)
- Vehicle ordered - Сколько пользователь арендовал автомобилей за время существования аккаунта
- Vehicles shared - Сколько заказов автомобилей пользователя было произведено за время существования аккаунта

Также, на этой странице можно просматривать такую информацию, как:

1. Активные заказы
   - Id заказа
   - Название арендованного автомобиля
   - Оплаченное время
   - Оставшееся время
   - Время окончания заказа
2. Добавленные автомобили
   - Id автомобиля
   - Название автомобиля
   - Цена автомобиля в формате (час - день)
   - Дата добавления
   - Количество раз, которое автомобиль был арендован другими пользователями
   - Статус автомобиля
   
При работе с таблицей автомобилей, можно наблюдать 3 цвета:

- Жёлтый - Автомобиль добавлен, но не отображается в каталоге
- Белый - Автомобиль добавлен в каталог и ожидает своего арендатора
- Зелётный - Автомобиль был арендован другим пользователем

В зависимости от состояния и цвета автомобиля, пользователю открываются/блокируются новые/старые функции

### Страница с редактированием информации активного пользователя

Как было сказано выше, для авторизованного в систему пользователя появляется возможность заходить в свой личный кабинет и отслеживать всю информацию, связанную с его заказами, а также автомобилями, которые данный пользователь предоставил для пользования другим пользователям нашего сервиса. Но что, если какая-то информация была введена не совсем корректно, если данные пользователя поменялись... А может быть опубликовавший автомобиль пользователь решит каким-то образом `выделться` на фоне других??? Для решения этих задач мной были разработаны страницы с редактированием информации как о текущем пользователе, так и о его автомобилях:

Из своего личного кабинета, перейти на страницу с редактированием информации о себе, пользователь может при помощи нажатия на соответствующую кнопку `Edit Profile`

![1](https://user-images.githubusercontent.com/55713244/204080966-3fb3e496-bde6-4dd4-85c5-61ebcef9350d.jpg)

При нажатии, пользователю открывается страница, на которой он может менять любую информацию, что отображена на странице

![2](https://user-images.githubusercontent.com/55713244/204080997-bd52eb58-3d2c-4654-b9a6-6ec9ca55a91b.jpg)

В частности: аватар своего профиля (на выбор 7 изображений из предложенных (1 default-ная, которая присваивается каждому новому пользователю, и 6 других на выбор))

![3](https://user-images.githubusercontent.com/55713244/204219430-78603c08-a100-471e-b04f-711709092ca6.jpg)

Также можно поменять свой пароль. Установление нового пароля я сделал таким образом, что вводить предыдущий не понадобится (в реальности же, необходимо было бы запрашивать реальный пароль перед тем, как устанавливать новый)

![4](https://user-images.githubusercontent.com/55713244/204219981-0767aa7e-e719-49dd-b530-15b33febc6ff.jpg)

Из простого, пользователь может менять любое из полей на данной странице (например, описание профиля, свой номер телефона, e-mail и тд). После того, как все интересующие пользователя поля были изменены, необходимо нажать на соответствующую кнопку `Save Changes` (новый пароль устанавливается автоматически при нажатии кнопки `Save` на странице с установлением нового пароля). Если была произведена какая-то ошибка при заполнении полей, пользователь может нажать на кнопку `Cancel Chnages`, что вернёт страницу в её первоначальный вид (тобиш к старым данным), либо же нажать на кнопку `Get Back`, которая перенесёт его обратно в свой личный кабинет.

### Страница с редактированием информации автомобиля активного пользователя

Собственно, касаемо автомобилей, пользователь также может менять ту или иную информацию, преведенную на странице:

![5](https://user-images.githubusercontent.com/55713244/204221476-e33e50c0-38c7-4afc-8839-65569e15cc1b.jpg)

Однако, количество доступных полей для изменения в несколько раз меньше, чем у пользователя. Почему так? Дело в том, что при разработке, мне в голову пришла мысль: У каждого автомобиля ведь отслеживается число заказов. А что, если пользователь мог бы менять автомобиль полностью, скажем, поменять его изображение и название? Рейтинг у этого автомобиля остался бы тот же, а вот детальное описание полностью бы изменилось. Как по мне, такой подход смог бы играть ключевую роль в определении другими пользователями, какой автомобиль ему выбрать: например, пользователь, разместивший свой автомобиль год назад, явно проигрывал бы пользователю, который поменял бы описание своего старого автомобиля на описание более нового. Таким образом, мной был сделан вывод, что информацию нужно скрыть от редактирования, а доступными оставить только самые необходимые поля: поля с тарифом, описанием, а также местоположением.

### Страница с оформлением нового заказа на аренду автомобиля

Дюбой из авторизованных пользователей может просматривать детальную информацию об автомобилях других пользователей в каталоге. Со страницы каждого автомобиля, пользователь может перейти на страницу с оформлением заказа на пользование этим автомобилем:

![6](https://user-images.githubusercontent.com/55713244/204223806-ddc05126-ab72-499d-9e18-93c26320b20d.jpg)

На этой странице ему предлагается выбрать временной промежуток, в который он сможет пользоваться автомобилем. 

![7](https://user-images.githubusercontent.com/55713244/204224132-66ec1550-9e33-4679-971e-508c5b9bfac4.jpg)

Выбор промежутка времени осуществляется с помощью элемента `daterangepicker`, в котором мной было установлено: Время начала - округление в сторону ближайшего часа. Время конца (default-ное) - время начала + 1 час. Таким образом, минимальное время для использования авто - 1 час. Макисмальное же время, которое я поставил - 7 дней со времени начала. Подсчёт суммы к оплате же происходит по формуле: Число дней умноженное на тариф/день + число часов * тариф/час. Как только пользователь сделает все необходимые приготовления и подтвердит заказ - он будет перенаправлен на страницу с оплатой:

![8](https://user-images.githubusercontent.com/55713244/204225115-b44bc192-284f-4d55-b590-9ef51475d537.jpg)

На странице с оплатой приведена вся информация, необходимая пользователю для того, чтоб ещё раз перепроверить свой заказ. Как только оплата будет произведена, пользователи смогут найти автомобили в их личном кабинете: Пользователь, который владеет автомобилем - увидит, что автомобиль был арендован, а пользователь, что арендовал автомобиль - увидит полную информацию об оплаченном времени а также всю информацию касательно оставшегося времени и времени окончания аренды на этот автомобиль. Выглядит это следующим образом:

![1](https://user-images.githubusercontent.com/55713244/204793945-6765988d-2567-4a1a-8b1d-2d6f316aee25.jpg)

Когда пользователь решит завершить свой заказ досрочно (заказ завершается не системой, отвечаемой за просроченные заказы, а самим пользователм, совершившим заказ), у него появляется возможность выставить рейтинг для автомобиля, которым он пользовался на протяжении N-го промежутка времени:

![2](https://user-images.githubusercontent.com/55713244/204793971-bc470493-0d0d-401b-9266-a9fd235ba739.jpg)

Опять же, у пользователя есть выбор: Поставить рейтинг автомобилю, после чего нажать на соответствующую кнопку `Submit and Finish` и завершить заказ вместе с рейтингом, либо же пропустить шаг с выставлением рейтинга, нажав на кнопку `Finish and not submit`, и завершить заказ без отправки рейтинга. 

Сам же рейтинг можно увидеть на странице с информацией об автомобиле. В зависимости от поставленыых пользователями оценок, на странице будет отображаться общая статистика. Автомобиль с 0 звёздами можно было увидеть выше, а страница с автомобилем с проставленным рейтингом от одного пользователя показана ниже:

![3](https://user-images.githubusercontent.com/55713244/204796114-ef6a58fb-4371-462d-bba9-42520cd84d80.jpg)

## Техническая документация

В данном разделе я постараюсь описать, а как проект устроен изнутри, что происходит внутри <Чёрного ящика>. Это поможет лучше разобраться в проекте тем, кто хоть немного имеет представление о языке программирования C# и аспекты программирования на нём.

1) **Хранилище автомобилей**

Конечно, для того, чтобы работать с какими-либо данными, для начала, нужно определиться, а как именно их хранить? Конечно, супер крупные проекты используют для хранения такие БД, как Oracle, PostgreSQL, MySQL, MSSQL и другие. Однако, проблема такого подхода состоит в том, что доступ к приложениям, привязанным к базам данных, можно получить только в том случае, если подключение к той самой базе данных может быть получено на устройстве пользователя (За такую функцию приходится платить крупные суммы денег компаниям-гигантам). Так как я не располагаю большими суммами денег, для возможности любому пользователю запускать проект и не испытывать проблемы с его тестированием и пользованием, мной была выдвинута идея использования сериализации и десериализации в контекстах относительных путей. Таким образом, проблем с получением доступа у пользователей, скачавших приложение из репозитория, возникнуть не должно.

**Каким же образом происходит получение данных из JSON-файлов и как вообще устроено получение данных? (Для всех локальных хранилищ)**

Для того, чтобы не привязываться к какому-то определённому типу хранилищ, мной был применён подход Dependency Injection, а именно внедрение Singleton зависимостей между хранилищами и локальным репозиторием программы. То есть: в моей программе есть интерфейс:
```C#
    public interface IVehiclesRepository
```
Этот интерфейс будет являться связующим звеном для получения данных. В данном интерфейсе есть набор методов, которые должны быть реализованы для того или иного сервиса, чтоб им можно было пользоваться независимо от реализации методов. Главное, чтоб эти методы были реализованы в том классе, который решит реализовать этот интерфейс. Для реализации локального интерфейса, так как я исользую локальное хранилище данных в файлах, а не в БД, я решил использовать Singleton подход, что будет означать, что объект сервиса будет создаваться только при первом обрпщении к нему, после чего все последующие запросы будут проходить через тот самый сервис. Ниже, мы явно указываем, что если кто-то захочет получить объект этого сервиса через интерфейс, мы дадим ему конкретную реализацию (В данном случае реализацию локального репозитория):
```C#
builder.Services.AddSingleton<IVehiclesRepository, VehiclesLocalRepository>(); // Где требуется IVehiclesRepository - дай реализацию VehiclesLocalRepository
```
Сами же локальные репозитории реализованы таким образом, чтоб уменьшить количество загрузок из файлов в само приложение. Как только происходит первое обращение к сервису - производится работа метода SetUpLocalRepository. Этот метод запишет в путой объект List<> модели, считанные из JSON - файла, после чего все манипуляции будут проходить непосредственно через сам этот List<>, а если данные будут меняться - будет вызываться асинхронный метод SaveChanges(), который призван асинхронно записать изменения в файл (Повторное считывание файла не происходит, так как мы работаем с объектом List<>, который мы также изменили перед тем, как запрашивать обновление JSON-файла). Таким образом, применённый мной подход не только позволит пользователю получать доступ в кратчайшие сроки, но и позволит избежать излишних нагрузок системы для загрузки данных из файлов каждый раз при обращении к сервису.

2) **От чего вообще зависит то, какие автомобили видит пользователь как на главной странице(в виде маркеров), так и на странице каталога?**

При проектировнии, мной была выявлена следующая проблема - А должен ли пользователь, добавивший автомобиль в каталог, иметь возможность взаимодействовать с тем же самым автомобилем из каталога? Конечно же нет. Такой подход позволит пользователю, добавившему автомобиль в каталог, арендовать свой же автомобиль (В чём смысл???). Для избежания этой проблемы, мной был выбран следующий подход:

Представим, что на данный момент, нет ни пользователей, ни добавленных автомобилей. Вот мы запускаем наше приложение. На главной странице мы видим карту с 0 маркерами, а в каталоге также 0 автомобилей. Мы создаём новый аккаунт, входим в него, делимся своим автомобилем. НО! Автомобиль не появляется в каталоге. В чём же проблема? Дело в том, что перед тем, как отобразить автомобиль в каталоге, добавивший его пользователь должен явно опубликовать автомобиль из своего личного кабинета путём нажатия кнопки `Publish` напротив выбранного автомобиля. Как только он произведёт публикацию, для этого пользователя в каталоге ничего не изменится, он также будет видеть 0 автомобилей. ОДНАКО! Если мы выйдем из аккаунта или создадим новый - В каталоге будет виднеться тот самый автомобиль, которым поделился и который опубликовал предыдущий пользователь. Таким образом, пользователь, добавивший автомобиль и разместивший его в каталог, не может увидеть свои же собственные автомобили (Все свои добавленные автомобили пользователь может увидеть в своём личном кабинете). ЗАТО! После того, как пользователь поделится своим автомобилем и разместит его в каталог из личного кабинета, хотя он не видит этот автомобиль в каталоге, он может перейти на главную страницу и обратить внимание, что его автомобиль появился на карте в виде маркера (Однако в каталоге его по-прежнему нет). Дело в том, что система показывает ЛЮБОМУ пользователю ВСЕ автомобили на карте в виде маркера, которые были ОПУБЛИКОВАНЫ в личном кабинете не зависимо от того, какой пользователь сейчас находится в системе. Однако в каталоге, система показывает те же самые автомобили, что изображены на карте на главной странице в виде маркеров, НО ещё проверяется условие, что ID владельца автомобиля не равно ID текущего пользователя, вошедшего в аккаунт. Для неавторизованного пользователя ID не проверяется. Ему показываются те же самые автомобили в каталоге, что и на карте на главной странице. 

Таким образом, подводя итог:
- Автомобиль появится на карте и в каталоге только в том случае, если пользователь, поделившись автомобилем, опубликует его из своего личного кабинета путём нажатия кнопки `Publish` (И пропадёт, если он решит убрать его из каталога путём нажатия кнопки `Hide`)
- Неавторизованному пользователю (на карте и в каталоге) видны только те автомобили, которые были опубликованы пользователями из личного кабинета
- Авторизованному пользователю (на карте) видны только те автомобили, которые были опубликованы пользователями из личного кабинета
- Авторизованному пользователю (в каталоге) видны только те автомобили, которые были опубликованы пользователями из личного кабинета и которые не относятся к текущему пользоватлю

3) **Добавление своего автомобиля авторизованным пользователем**

При добавлении автомобиля, на странице использованы такие технологии, как локальное хранилище данных (Local Storage) для хранения координат местоположения автомобиля, а также специальные скрипты и элемент C# `IFormFile` для получения файла изображения со сраницы. 

**Local Storage**

Предположим, что мы - очень невнимательные пользователи, которые всё время допускают ошибки на страницах. Для того, чтобы избежать повторного ввода координат каждый раз при обновлении страницы, пной было принято решение хранить координаты GoogleMaps в локальном хранилище и чистить эти данные в случае покидания пользователем страницы добавления своего автомобиля. Так как данные, применяемые при работе с GoogleMaps являются специфическими (представление типа данных float отличаются знаком разделителя) чтоб избежать большого количества манипуляции с данными, используется локальное хранилище, которое призвано сократить число операция приведения данных из одного представления в другое.

**IFormFile и скрипты**

При работе с изображеним, мы не можем получать доступ к файловой системе пользователя со страницы Razor, так как это просто недопустимо в рамках работы системы безопасности, поэтому приходится использовать определённые подходы, например, как работа с IFormFile. Объекты этого типа хранят всю необходимую информацию о выбранном изображении, которое выберет пользователь, при этом не представляет никакой угрозы для файловой системы компьютера в целом. Однако использование такого подхода имеет недотаток - пользователю необходимо каждый раз выбирать изображение снова и снова, если пользователем повторно допускаются ошибки на странице добавления автомобиля.

## Технические аспекты проекта (проблемы и их решения)

Ниже я постарался описать интересные технические моменты, которые я предпринимал в течение проектирования и разработки проекта. 

### Проблемы, с которыми столкнулся во время разработки и какой план действий принмал
#### Работа со страницей для отображения каталога и изменения параметров отображения (фильтры, кол-во автомобилей на странице и тд.)

При работе с получением автомобилей из репозитория у меня было 2 идеи, как решить данную задачу: Либо при каждом обращении к контроллеру формировать новый объект с информацией о машинах и передавать его во `View`, либо же принимать этот объект из `View`, если он уже был передан однажды, при этом не делая никаких дополнительных запросов в репозиторий, и модифицировать согласно предпочтениям пользователя по количеству отображения автомобилей на странице и тд. Сначала я принял решение пойти через получение модели из `View`, однако столкнулся с такой проблемой, как `Model Binding`, которая просто так не даёт получить `List` переданных в качестве модели автомобилей: Либо нужно использовать `Ajax`, при этом заранее сериализовать модель в `JSON` и после чего десериализовать полученную `JSON`-строку в `Controller-е`, либо никак) Поэтому я выбрал первый путь (через создание объекта), так как в этом случае придётся манипулировать в основном со ссылками на данные, нежели чем с самими данными. (p.s. Также, работа через первый подход потребовала бы накладных расходов на проверку, а не добавилась ли в репозиторий новая машина, но уже другим пользователем, и если добавилась, также необходимо было бы добавить её в модельку, пришедшую из `View`)


#### Работа с заказами и общий принцип их оформления

Изначально, мной была заложена идея, что пользователь может как оформить заказ на определённое время, внеся предоплату, так и просросить его, за что потребуется внести дополнительные деньги за просроченное время. Однако, при дальнейшем развитии идеи, мной были выявлены следующие проблемы, касательно как программной, так и правовой идеи такого подхода:

- Что, если пользователь возьмёт автомобиль в аренду, и больше не будет входить в аккаунт (скажем, год. Это приведёт к переполнению типа при отслеживании суммы оплаты дополнительного времени. В таком случае необходимо было бы ввести ограничение на просроченное время, но в таком случае, что делать с просроченным заказом, если пользователь не сможет оплатить его, и что делать с этим автомобилем??? Выставить мы его обрано в каталог также не можем, тк мы не уверены, что с ним и с заказом в итоге произошло)
- Что, если пользователь просто забудет закончить заказ? Как это будет решаться в правовом поле?
- Что, если пользователь просрочит, закончит, но не оплатит сумму за просроченное время? Как долго этот заказ будет висеть, как регулировать действия пользоватля при наличии хотя бы одного не оплаченного заказа??
- И другие проблемы, которые появились при использовании этого подхода...

Поэтому, мной был выбран следующий план действий: Подразумевается, что заказ начинается, как только пользователь оформляет заказ на пользование авто и оплачивает его, а заканчивается этот же заказ ровно тогда, когда время пользования станет равно оплаченному времени. После чего, автомобиль сразу становится доступным для других пользователей в каталоге, а нынешний заказ пропадает с личного кабинета пользователя, оплатившего время на его использование. Если другой пользователь оформит заказ на тот же автомобиль и при прибытии на место обнаружит, что автомобиль отсутствует на том месте, на котором он расположен на карте, он имеет право подать в суд на человека, который просрочил своё время (собственно как и заказчик). Таким образом, каждый пользователь, который оформляет заказ, берёт на себя ответственность закончить его во время.
