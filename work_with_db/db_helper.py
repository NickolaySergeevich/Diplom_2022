from __future__ import annotations

from typing import Optional

import mysql.connector


class DBHelper:
    """
    Класс для работы с удалённой бд (MySQL)
    Использует паттерн singleton
    """

    __instance = None  # Объект в единственном виде
    __connection = None  # Объект подключения к бд
    __cursor = None  # Курсор в бд

    settings_file = str()  # Путь до файла с настройками

    def __init__(self, user: str, password: str, host: str, database_name: str):
        """
        Конструктор стандартный для создания подключения и получения курсора

        :param user: Пользователь
        :param password: Пароль
        :param host: Хост
        :param database_name: Имя базы данных
        """

        if not DBHelper.__instance:
            self.__connection = mysql.connector.connect(
                user=user, password=password,
                host=host, database=database_name
            )
            self.__cursor = self.__connection.cursor()

            DBHelper.__instance = self

    @staticmethod
    def get_instance() -> DBHelper:
        """
        Получение текущего объекта - он может быть только один!

        :return: Объект типа DBHelper
        """

        if not DBHelper.__instance:
            DBHelper(**DBHelper.read_settings_file())

        return DBHelper.__instance

    def __del__(self):
        """
        Очистка подключений

        :return: Ничего
        """

        DBHelper.get_instance().__connection.close()

    @staticmethod
    def get_user_id(username: str) -> Optional[int]:
        """
        Получение id пользователя по логину

        :param username: Имя пользователя

        :return: ID пользователя или None
        """

        try:
            query = f"select id from users where username = '{username}'"
            DBHelper.get_instance().__cursor.execute(query)
            data = DBHelper.get_instance().__cursor.fetchall()
            if len(data) == 0:
                return None
            else:
                return data[0][0]
        except mysql.connector.errors.IntegrityError:
            return None

    @staticmethod
    def get_nast_id(username: str) -> Optional[int]:
        """
        Получить id наставника

        :param username: Имя наставника

        :return: ID наставника или None
        """

        try:
            query = f"select id from users where username = '{username}' and users_role_id = 2"
            DBHelper.get_instance().__cursor.execute(query)
            data = DBHelper.get_instance().__cursor.fetchall()
            if len(data) == 0:
                return None
            else:
                return data[0][0]
        except mysql.connector.errors.IntegrityError:
            return None

    @staticmethod
    def get_tasks() -> tuple:
        """
        Получение списка задач

        :return: Кортеж с задачами
        """

        what_need = (
            "id", "name", "organization", "description", "teams_count", "team_member_max", "region", "essay", "test")

        DBHelper.get_instance().__cursor.execute(
            ("select " + "{}, " * (len(what_need) - 1) + "{} from tasks").format(*what_need)
        )

        answer_list = list()
        for elem in DBHelper.get_instance().__cursor.fetchall():
            answer_list.append(dict([(what_need[i], elem[i]) for i in range(len(what_need))]))

        return tuple(answer_list)

    @staticmethod
    def get_tasks_by_user(user_id: int) -> tuple:
        """
        Получение списка задач для пользователя

        :param user_id: ID пользователя

        :return: Кортеж с задачами
        """

        query = f"select tasks.id, tasks.name, tasks.organization, users_to_task.name, users_to_task.is_team_lead " \
                f"from tasks " \
                f"left join users_to_task " \
                f"on tasks.id = users_to_task.task_id " \
                f"where users_to_task.user_id = {user_id}"
        DBHelper.get_instance().__cursor.execute(query)

        answer_list = list()
        for elem in DBHelper.get_instance().__cursor.fetchall():
            answer_list.append({"id": elem[0], "name": elem[1], "organization": elem[2], "command_name": elem[3],
                                "is_team_lead": elem[4]})

        return tuple(answer_list)

    @staticmethod
    def get_chat(username_from: str, username_to: str) -> tuple:
        """
        Получение чата для конкретного пользователя

        :param username_from: Имя пользователя, от которого пришли сообщения.
        :param username_to: Имя пользователя, которому пришли сообщения.

        :return: Кортеж диалога
        """

        # От первого параметра многое зависит
        # Если менять его - менять цикл!
        what_need = ("users.username", "chats.text", "chats.date")
        query = ("select " + "{}, " * (len(what_need) - 1) + "{} " +
                 "from users one, users two, chats " +
                 "left join users " +
                 "on chats.user_from = users.id " +
                 "where " +
                 "(one.username = '{}' and (chats.user_from = one.id or chats.user_to = one.id)) and " +
                 "(two.username = '{}' and (chats.user_from = two.id or chats.user_to = two.id))").format(*what_need,
                                                                                                          username_from,
                                                                                                          username_to)

        DBHelper.get_instance().__cursor.execute(query)

        answer_list = list()
        for elem in DBHelper.get_instance().__cursor.fetchall():
            is_mine = False
            if elem[0] == username_from:
                is_mine = True

            answer_list.append({"mine": is_mine})
            answer_list[-1].update(**dict([(what_need[i], elem[i]) for i in range(1, len(what_need))]))

        return tuple(answer_list)

    @staticmethod
    def login_in(username: str, password: str) -> Optional[dict]:
        """
        Вход пользователя

        :param username: Логин пользователя
        :param password: Пароль пользователя

        :return: Словарь с информацией или None
        """

        query = f"select id, users_role_id from users where username = '{username}' and password = '{password}'"
        DBHelper.get_instance().__cursor.execute(query)

        data = DBHelper.get_instance().__cursor.fetchall()
        if len(data) == 0:
            return None
        else:
            return {"id": data[0][0], "users_role_id": data[0][1]}

    @staticmethod
    def get_user_information(user_id: int, username: str, password: str) -> Optional[dict]:
        """
        Получение информации о пользователе

        :param user_id: ID пользователя
        :param username: Логин пользователя
        :param password: Пароль пользователя (MD5)

        :return: Кортеж с информацией или None
        """

        if DBHelper.get_instance().login_in(username, password) is None:
            return None

        query = f"select name, surname, patronymic, country, city, educational_institution, class, email, " \
                f"phone_number from users_info where user_id = {user_id}"
        DBHelper.get_instance().__cursor.execute(query)

        data = DBHelper.get_instance().__cursor.fetchall()
        if len(data) == 0:
            return None
        else:
            return {
                "user_id": user_id,
                "username": username,
                "name": data[0][0],
                "surname": data[0][1],
                "patronymic": data[0][2],
                "country": data[0][3],
                "city": data[0][4],
                "educational_institution": data[0][5],
                "class_number": data[0][6],
                "email": data[0][7],
                "phone_number": data[0][8]
            }

    @staticmethod
    def registration_expert(username: str, password: str, name: str, surname: str, patronymic: str, email: str,
                            phone_number: str, organization: str, city: str) -> bool:
        """
        Регистрация эксперта

        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param name: Имя пользователя
        :param surname: Фамилия пользователя
        :param patronymic: Отчество пользователя
        :param email: Почта пользователя
        :param phone_number: Телефон пользователя
        :param organization: Организация пользователя
        :param city: Город пользователя

        :return: Успешно ли
        """

        try:
            query = f"insert into users(username, password, users_role_id) values('{username}', '{password}', {5})"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            return False

        try:
            query = f"insert into experts_info " \
                    f"(user_id, name, surname, patronymic, email, phone_number, organization, city) " \
                    f"values " \
                    f"({DBHelper.get_instance().__cursor.lastrowid}, '{name}', '{surname}', '{patronymic}', " \
                    f"'{email}', '{phone_number}', '{organization}', '{city}')"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            DBHelper.get_instance().__connection.rollback()
            return False

        DBHelper.get_instance().__connection.commit()
        return True

    @staticmethod
    def registration_org(username: str, password: str, name: str, surname: str, patronymic: str, email: str) -> bool:
        """
        Регистрация оргкомитета

        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param name: Имя пользователя
        :param surname: Фамилия пользователя
        :param patronymic: Отчество пользователя
        :param email: Почта пользователя

        :return: Успешно ли
        """

        try:
            query = f"insert into users(username, password, users_role_id) values('{username}', '{password}', {3})"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            return False

        try:
            query = f"insert into organizing_committee_info " \
                    f"(user_id, name, surname, patronymic, email) " \
                    f"values ({DBHelper.get_instance().__cursor.lastrowid}, '{name}', '{surname}', " \
                    f"'{patronymic}', '{email}')"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            DBHelper.get_instance().__connection.rollback()
            return False

        DBHelper.get_instance().__connection.commit()
        return True

    @staticmethod
    def registration_partners(username: str, password: str, name: str, surname: str, patronymic: str, email: str,
                              phone_number: str, organization: str, city: str) -> bool:
        """
        Регистрация партнёра

        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param name: Имя пользователя
        :param surname: Фамилия пользователя
        :param patronymic: Отчество пользователя
        :param email: Почта пользователя
        :param phone_number: Телефон пользователя
        :param organization: Организация пользователя
        :param city: Город пользователя

        :return: Успешно ли
        """

        try:
            query = f"insert into users(username, password, users_role_id) values('{username}', '{password}', {4})"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            return False

        try:
            query = f"insert into partners_info " \
                    f"(user_id, name, surname, patronymic, email, phone_number, organization, city) " \
                    f"values " \
                    f"({DBHelper.get_instance().__cursor.lastrowid}, '{name}', '{surname}', '{patronymic}', " \
                    f"'{email}', '{phone_number}', '{organization}', '{city}')"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            DBHelper.get_instance().__connection.rollback()
            return False

        DBHelper.get_instance().__connection.commit()
        return True

    @staticmethod
    def registration_user(username: str, password: str, name: str, surname: str, patronymic: str, country: str,
                          city: str, educational_institution: str, class_number: int, email: str,
                          phone_number: str) -> bool:
        """
        Регистрация участника

        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param name: Имя пользователя
        :param surname: Фамилия пользователя
        :param patronymic: Отчество пользователя
        :param country: Страна пользователя
        :param city: Город пользователя
        :param educational_institution: Место обучения пользователя
        :param class_number: Номер класса пользователя
        :param email: Почта пользователя
        :param phone_number: Телефон пользователя

        :return: Успешно ли
        """

        try:
            query = f"insert into users(username, password, users_role_id) values('{username}', '{password}', {1})"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            return False

        try:
            query = f"insert into users_info " \
                    f"(user_id, name, surname, patronymic, country, city, educational_institution, class, " \
                    f"email, phone_number) " \
                    f"values " \
                    f"({DBHelper.get_instance().__cursor.lastrowid}, '{name}', '{surname}', '{patronymic}', " \
                    f"'{country}', '{city}', '{educational_institution}', {class_number}, '{email}', '{phone_number}')"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            DBHelper.get_instance().__connection.rollback()
            return False

        DBHelper.get_instance().__connection.commit()
        return True

    @staticmethod
    def update_user(user_id: int, username: str, password: str, name: str, surname: str, patronymic: str, country: str,
                    city: str, educational_institution: str, class_number: int, email: str,
                    phone_number: str) -> bool:
        """
        Обновление данных участника

        :param user_id: ID пользователя
        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param name: Имя пользователя
        :param surname: Фамилия пользователя
        :param patronymic: Отчество пользователя
        :param country: Страна пользователя
        :param city: Город пользователя
        :param educational_institution: Место обучения пользователя
        :param class_number: Номер класса пользователя
        :param email: Почта пользователя
        :param phone_number: Телефон пользователя

        :return: Успешно ли
        """

        try:
            query = f"select id from users where id = {user_id} and password = '{password}'"
            DBHelper.get_instance().__cursor.execute(query)

            if len(DBHelper.get_instance().__cursor.fetchall()) == 0:
                return False
        except mysql.connector.errors.IntegrityError:
            return False

        try:
            query = f"update users set username = '{username}' where id = {user_id}"
            DBHelper.get_instance().__cursor.execute(query)

            query = f"update users_info set name = '{name}', surname = '{surname}', patronymic = '{patronymic}', " \
                    f"country = '{country}', city = '{city}', educational_institution = '{educational_institution}', " \
                    f"class = {class_number}, email = '{email}', phone_number = '{phone_number}' " \
                    f"where user_id = {user_id}"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            DBHelper.get_instance().__connection.rollback()
            return False

        DBHelper.get_instance().__connection.commit()
        return True

    @staticmethod
    def registration_nast(username: str, password: str, name: str, surname: str, patronymic: str, country: str,
                          city: str, educational_institution: str, email: str, phone_number: str) -> bool:
        """
        Регистрация наставника

        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param name: Имя пользователя
        :param surname: Фамилия пользователя
        :param patronymic: Отчество пользователя
        :param country: Страна пользователя
        :param city: Город пользователя
        :param educational_institution: Место обучения пользователя
        :param email: Почта пользователя
        :param phone_number: Телефон пользователя

        :return: Успешно ли
        """

        try:
            query = f"insert into users(username, password, users_role_id) values('{username}', '{password}', {2})"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            return False

        try:
            query = f"insert into users_info " \
                    f"(user_id, name, surname, patronymic, country, city, educational_institution, class, " \
                    f"email, phone_number) " \
                    f"values " \
                    f"({DBHelper.get_instance().__cursor.lastrowid}, '{name}', '{surname}', '{patronymic}', " \
                    f"'{country}', '{city}', '{educational_institution}', -1, '{email}', '{phone_number}')"
            DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            DBHelper.get_instance().__connection.rollback()
            return False

        DBHelper.get_instance().__connection.commit()
        return True

    @staticmethod
    def sign_up_to_task(users_id: tuple, task_id: int, nast_id: int, command_name: str, is_team_lead_id: int) -> bool:
        """
        Записывает участников на конкурс

        :param users_id: Кортеж пользовательских id
        :param task_id: ID конкурса
        :param nast_id: ID наставника
        :param command_name: Название команды
        :param is_team_lead_id: Кто тимлидер

        :return: Успешно ли
        """

        try:
            query = f"select teams_count, teams_exist from tasks where id={task_id}"
            DBHelper.get_instance().__cursor.execute(query)
            data = DBHelper.get_instance().__cursor.fetchall()
            if data[0][0] == data[0][1] and data[0][0] != -1:
                return False
        except mysql.connector.errors.IntegrityError:
            return False

        try:
            for user_id in users_id:
                if user_id == is_team_lead_id:
                    query = f"insert into users_to_task (user_id, task_id, nast_id, name, is_team_lead) " \
                            f"values ({user_id}, {task_id}, {nast_id}, '{command_name}', 1)"
                else:
                    query = f"insert into users_to_task (user_id, task_id, nast_id, name, is_team_lead) " \
                            f"values ({user_id}, {task_id}, {nast_id}, '{command_name}', 0)"
                DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            DBHelper.get_instance().__connection.rollback()
            return False

        query = f"update tasks set teams_exist = teams_exist + 1 where id = {task_id}"
        DBHelper.get_instance().__cursor.execute(query)

        DBHelper.get_instance().__connection.commit()

        return True

    @staticmethod
    def remove_from_task(task_id: int, command_name: str) -> bool:
        """
        Удаляет команду с конкурса

        :param task_id: ID конкурса
        :param command_name: Название команды

        :return: Успешно ли
        """

        query = f"delete from users_to_task where name = '{command_name}'"
        DBHelper.get_instance().__cursor.execute(query)
        if DBHelper.get_instance().__cursor.rowcount == 0:
            return False

        query = f"update tasks set teams_exist = teams_exist - 1 where id = {task_id}"
        DBHelper.get_instance().__cursor.execute(query)

        DBHelper.get_instance().__connection.commit()

        return True

    @staticmethod
    def get_users_with_teams() -> dict:
        """
        Находит списки команд и удобно пакует

        :return: Хороший словарик
        """

        query = "select users_info.name, users_info.surname, users_info.patronymic, users_info.email, " \
                "users_to_task.is_team_lead, users_to_task.name, tasks.name from users_info " \
                "left join users_to_task on users_info.user_id = users_to_task.user_id " \
                "left join tasks on tasks.id = users_to_task.task_id " \
                "where users_to_task.name IS NOT NULL"
        DBHelper.get_instance().__cursor.execute(query)

        return_dict = dict()
        for elem in DBHelper.get_instance().__cursor.fetchall():
            if elem[-1] not in return_dict.keys():
                return_dict[elem[-1]] = dict()
            if elem[-2] not in return_dict[elem[-1]].keys():
                return_dict[elem[-1]][elem[-2]] = list()
            return_dict[elem[-1]][elem[-2]].append({
                "name": elem[0],
                "surname": elem[1],
                "patronymic": elem[2],
                "email": elem[3],
                "is_team_lead": bool(elem[4])
            })

        return return_dict

    @staticmethod
    def read_settings_file() -> dict:
        """
        Читает файл с настройками

        :return: Словарь с настройками
        """

        if DBHelper.settings_file == str():
            raise FileNotFoundError

        settings = dict()

        with open(DBHelper.settings_file, 'r') as settings_file:
            for line in settings_file:
                line_list = line.split('=')
                settings[line_list[0]] = line_list[1][:-1]  # Убираем перенос строки

        return settings


def main() -> None:
    """
    Для тестов

    :return: Ничего
    """

    # DBHelper.settings_file = "help_files/database_settings.dk"

    # print(DBHelper.get_instance().get_user_id("asakura"))
    # print(DBHelper.get_instance().get_nast_id("asakuraBig"))
    # print(DBHelper.get_instance().get_tasks())
    # print(DBHelper.get_instance().get_tasks_by_user(14))
    # print(DBHelper.get_instance().get_chat("asakura", "dasha"))
    # print(DBHelper.get_instance().login_in("asakura", "78e5233d20f3608ebc410ee2c18a41da"))
    # print(DBHelper.get_instance().get_user_information(14, "asakura", "78e5233d20f3608ebc410ee2c18a41da"))
    # print(DBHelper.get_instance().registration_expert("testExpert", "78e5233d20f3608ebc410ee2c18a41da", "Test",
    #                                                   "Test", "Test", "test@gmail.com", "11111111111",
    #                                                   "Test", "Test"))
    # print(DBHelper.get_instance().registration_org("TestOrg", "78e5233d20f3608ebc410ee2c18a41da", "Test", "Test",
    #                                                "Test", "test@gmail.com"))
    # print(DBHelper.get_instance().registration_partners("testPartner", "78e5233d20f3608ebc410ee2c18a41da", "Test",
    #                                                     "Test", "Test", "test@gamil.com", "11111111111", "test",
    #                                                     "test"))
    # print(DBHelper.get_instance().registration_user("TestUser", "78e5233d20f3608ebc410ee2c18a41da", "Test", "Test",
    #                                                 "Test", "Test", "Test", "Test", 5, "test@gamil.com",
    #                                                 "11111111111"))
    # print(DBHelper.get_instance().update_user(48, "change", "5A50814F0E1E68543505DE83B28D4633", "change", "change",
    #                                           "change", "change", "change", "change", 5, "change@gmail.com",
    #                                           "81231231212"))
    # print(DBHelper.get_instance().registration_nast("TestNast", "78e5233d20f3608ebc410ee2c18a41da", "Test", "Test",
    #                                                 "Test", "Test", "test", "Test", "test@ya.ru", "22222222222"))
    # print(DBHelper.get_instance().sign_up_to_task((14, 19, 20, 30), 5, 31, "Test", 14))
    # print(DBHelper.get_instance().remove_from_task(5, "Test"))
    # print(DBHelper.get_instance().get_users_with_teams())


if __name__ == '__main__':
    main()
