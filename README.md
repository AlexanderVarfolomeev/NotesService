<h1>Ежедневник</h1>
Инструкция по запуску проекта

<h3>Обычный запуск:</h3>

Необходимо иметь установленный Microsoft SQL Server

1) Скачать проект
2) Запустить файл Notes.sln
3) Перейти в свойства проекта
4) Назначить несколько запускаемых проектов: Notes.Api, Notes.Identity, Notes.WPF
5) Запустить проект
6) Проект автоматически применит миграции и создаст бд с данными
7) Зарегистрировать аккаунт, войти по нему

<h3>Запуск с помощью Docker:</h3>

Необходимо иметь установленный Docker

1) Скачать проект
2) Запустить файл Notes.sln
3) Ввести в терминал команду: docker-compose up --build
4) Дождаться завершения работы и запустить проект Notes.WPF
5) Зарегистрировать аккаунт, войти по нему

<h3>Описание проекта:</h3>
<ul>
<li>СУБД - MS SQL Server</li>
<li>Архитектура: БД - WebAPI - WPF</li>
<li>При реализации WPF использовались паттерны: MVVM, Commands, Data bindings</li>
</ul>

Приложение, позволяющее сохранять информацию о планах и задачах и заносить их в ежедневник,
следить за графиком их распределения и выполнения в течение недели, собирать и отображать статистику активности пользователя.

Каждая заметка содержит следующую информацию:
<ul>
<li>Название</li>
<li>Дата начала, конца</li>
<li>Описание</li>
<li>Тип задачи - имеет названи и цвет, пользователь сам может добавлять типы, при удалении типа задачи получают пустое значение типа, цвет меняется, не учитываются в графике</li>
<li>Частота повторения (без, день, неделя, месяц, год)</li>
<li>Состояние (выполнена, ожидает, провалена (когда дата истекла))</li>
</ul>
Главный экран состоит из 3 вкладок:
<ul>
<li>Календарь, показывает список задач на текущую неделю</li>
<li>Типы задачи, показывает типы задачи, добавление удаление редактирование</li>
<li>График активности, показывает активность за последние 4 недели</li>
</ul>
