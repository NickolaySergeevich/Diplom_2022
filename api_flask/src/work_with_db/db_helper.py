import hashlib

import mysql.connector


class DBHelper:
    """
    Класс для работы с удалённой бд (MySQL)
    """

    def __init__(self, user: str, password: str, host: str, database_name: str):
        """
        Конструктор стандартный для создания подключения и получения курсора

        :param user: Пользователь
        :param password: Пароль
        :param host: Хост
        :param database_name: Имя базы данных
        """

        self._connection = mysql.connector.connect(user=user, password=password, host=host, database=database_name)
        self._cursor = self._connection.cursor()

    def __del__(self):
        """
        Очистка подключений

        :return: Ничего
        """

        self._connection.close()

    def get_users(self) -> list:
        """
        Возвращает список всех пользователей

        :return: Список с пользователями
        """

        self._cursor.execute(
            f"select * from users"
        )

        answer_list = list()
        for elem in self._cursor.fetchall():
            answer_list.append({
                "id": elem[0],
                "username": elem[1],
                "password (md5)": elem[2]
            })

        return answer_list

    def login_in(self, username: str, password: str, is_mdfive: bool = False) -> list | None:
        """
        Вход пользователя

        :param username: Логин пользователя
        :param password: Пароль пользователя
        :param is_mdfive: Хеширован ли пароль

        :return: Список с информацией или None
        """

        if not is_mdfive:
            password = hashlib.md5(password.encode("utf-8")).hexdigest()

        self._cursor.execute(
            f"select * from users where username = '{username}' and password = '{password}'"
        )
        answer_from_db = self._cursor.fetchall()
        if len(answer_from_db) != 0:
            self._cursor.execute(
                f"select * from users_info where user_id = '{answer_from_db[0][0]}'"
            )
            return self._cursor.fetchall()
        else:
            return None

    def registration(self, username: str, password: str, name: str, surname: str, is_mdfive: bool = False) -> bool:
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
            self._cursor.execute(
                f"insert into users (username, password) values ('{username}', '{password}')"
            )
            id_new_user = self._cursor.lastrowid
            self._cursor.execute(
                f"insert into users_info (user_id, name, surname)"
                f"values ({id_new_user}, '{name}', '{surname}')"
            )
            self._connection.commit()
        except mysql.connector.errors.IntegrityError:
            return False

        return True

    @staticmethod
    def read_settings_file(filename: str = "help_files/database_settings.dk") -> dict:
        """
        Просто читает файл с настройками

        :param filename: Имя файла

        :return: Словарь с настройками
        """

        settings = dict()

        with open(filename, 'r') as settings_file:
            for line in settings_file:
                line_list = line.split('=')
                settings[line_list[0]] = line_list[1][:-1]  # Убираем перенос строки

        return settings


def main() -> None:
    """
    Для тестов

    :return: Ничего
    """

    pass


if __name__ == '__main__':
    main()
