from __future__ import annotations

import hashlib
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
    def __select_from_users(what_need: tuple) -> tuple:
        """
        Делает запрос к таблице пользователей

        :param what_need: Какие столбцы нужны

        :return: Кортеж с ответом сервера
        """

        DBHelper.get_instance().__cursor.execute(
            ("select " + "{}, " * (len(what_need) - 1) + "{} from users").format(*what_need)
        )

        answer_list = list()
        for elem in DBHelper.get_instance().__cursor.fetchall():
            answer_list.append(dict([(what_need[i], elem[i]) for i in range(len(what_need))]))

        return tuple(answer_list)

    @staticmethod
    def get_users() -> tuple:
        """
        Возвращает кортеж всех пользователей\n
        (
            {
                "username": str,\n
                "password": str
            }
        )

        :return: Кортеж с пользователями
        """

        return DBHelper.get_instance().__select_from_users(("username", "password"))

    @staticmethod
    def get_users_name() -> tuple:
        """
        Возвращает кортеж имён всех пользователей\n
        ({"username": str})

        :return: Кортеж с именами пользователей
        """

        return DBHelper.get_instance().__select_from_users(("username",))

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
    def get_tasks() -> tuple:
        """
        Получение списка задач\n
        (
            {
                "name": str,\n
                "organization": str,\n
                "description": str,\n
                "teams_count": int or Null,\n
                "region": str or Null,\n
                "essay": bool or Null,\n
                "test": bool or Null
            }
        )

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
    def get_chat(username_from: str, username_to: str) -> tuple:
        """
        Получение чата для конкретного пользователя\n
        (
            {
                "mine": bool,\n
                "text": str,\n
                "date": datetime.datetime
            }
        )

        :param username_from: Имя пользователя, от которого пришли сообщения
        :param username_to: Имя пользователя, которому пришли сообщения

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
    def login_in(username: str, password: str, is_mdfive: bool = True) -> Optional[dict]:
        """
        Вход пользователя

        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param is_mdfive: Хеширован ли пароль

        :return: Словарь с информацией или None
        """

        if not is_mdfive:
            password = hashlib.md5(password.encode("utf-8")).hexdigest()

        what_need = ("users.id", "users_info.name", "users_info.surname", "users.username", "users.password")
        query = ("select " + "{}, " * (len(what_need) - 1) + "{} " +
                 "from users_info " +
                 "left join users " +
                 "on users_info.user_id = users.id " +
                 "where users.username = '{}' and users.password = '{}'").format(*what_need, username, password)

        DBHelper.get_instance().__cursor.execute(query)
        answer_from_db = DBHelper.get_instance().__cursor.fetchall()
        if len(answer_from_db) != 0:
            return dict([(what_need[i].split('.')[1], answer_from_db[0][i]) for i in range(len(what_need))])
        else:
            return None

    @staticmethod
    def registration(username: str, password: str, name: str, surname: str, is_mdfive: bool = True) -> bool:
        """
        Регистрация пользователя

        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param name: Имя пользователя
        :param surname: Фамилия пользователя
        :param is_mdfive: Хеширован ли пароль

        :return: Успешно ли
        """

        if not is_mdfive:
            password = hashlib.md5(password.encode("utf-8")).hexdigest()

        try:
            what_need = ("username", "password")
            query = ("insert into users (" + "{}, " * (len(what_need) - 1) + "{}) " +
                     "values (" + "'{}', " * (len(what_need) - 1) + "'{}')").format(*what_need, username, password)
            DBHelper.get_instance().__cursor.execute(query)

            what_need = ("user_id", "name", "surname")
            new_user_id = DBHelper.get_instance().__cursor.lastrowid
            query = ("insert into users_info (" + "{}, " * (len(what_need) - 1) + "{})" +
                     "values ({}, " + "'{}', " * (len(what_need) - 2) + "'{}')").format(*what_need,
                                                                                        new_user_id,
                                                                                        name, surname)
            DBHelper.get_instance().__cursor.execute(query)

            DBHelper.get_instance().__connection.commit()
        except mysql.connector.errors.IntegrityError:
            return False

        return True

    @staticmethod
    def sign_up_to_task(users_id: tuple, task_id: int) -> bool:
        """
        Записывает участников на конкурс

        :param users_id: Кортеж пользовательских id
        :param task_id: ID конкурса

        :return: Успешно ли
        """

        try:
            query = f"select teams_count, teams_exist from tasks where id={task_id}"
            DBHelper.get_instance().__cursor.execute(query)
            data = DBHelper.get_instance().__cursor.fetchall()
            if data[0][0] == data[0][1]:
                return False
        except mysql.connector.errors.IntegrityError:
            return False

        try:
            for user_id in users_id:
                query = f"insert into users_to_task (user_id, task_id) values ({user_id}, {task_id})"
                DBHelper.get_instance().__cursor.execute(query)
        except mysql.connector.errors.IntegrityError:
            DBHelper.get_instance().__connection.rollback()
            return False

        query = f"update tasks set teams_exist = teams_exist + 1 where id = {task_id}"
        DBHelper.get_instance().__cursor.execute(query)

        DBHelper.get_instance().__connection.commit()

        return True

    @staticmethod
    def remove_from_task(users_id: tuple, task_id: int) -> bool:
        """
        Удаляет команду с конкурса

        :param users_id: Кортеж пользовательских id
        :param task_id: ID конкурса

        :return: Успешно ли
        """

        for user_id in users_id:
            query = f"delete from users_to_task where user_id = {user_id}"
            DBHelper.get_instance().__cursor.execute(query)
        if DBHelper.get_instance().__cursor.rowcount == 0:
            return False

        query = f"update tasks set teams_exist = teams_exist - 1 where id = {task_id}"
        DBHelper.get_instance().__cursor.execute(query)

        DBHelper.get_instance().__connection.commit()

        return True

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

    # print(DBHelper.get_instance().get_users())  # Сделал!
    # print(DBHelper.get_instance().get_users_name())  # Сделал!
    # print(DBHelper.get_instance().get_tasks())  # Сделал!
    # print(DBHelper.get_instance().get_chat("dasha", "asakura"))  # Сделал!
    # print(DBHelper.get_instance().login_in("asakura", "78e5233d20f3608ebc410ee2c18a41da"))  # Сделал!
    # print(DBHelper.get_instance().registration("NS", "2545", "Nickolay", "Alekseev"))  # Сделал!
    # print(DBHelper.get_instance().sign_up_to_task((1, 2), 1))  # Сделал!
    # print(DBHelper.get_instance().remove_from_task((1, 2), 1))  # Сделал!
    # print(DBHelper.get_instance().get_user_id("baba")) Сделал!


if __name__ == '__main__':
    main()
