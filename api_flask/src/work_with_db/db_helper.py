import hashlib

import mysql.connector


class DBHelper:
    """
    Класс для работы с удалённой бд (MySQL)
    """

    # Настройки (лучше бы ваша бд была такой)
    _users_table = "users"

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

        self._cursor.close()
        self._connection.close()

    def check_user(self, username: str, password: str) -> bool:
        """
        Проверяет, есть ли пользователь в бд

        :param username: Имя пользователя.
        :param password: Пароль пользователя.

        :return: Есть в базе или нет
        """

        self._cursor.execute(
            f"select * from {DBHelper._users_table} "
            f"where username='{username}' and password='{hashlib.md5(password.encode()).hexdigest()}'"
        )

        return len(self._cursor.fetchall()) == 1

    @staticmethod
    def read_settings_file(filename: str = "../../help_files/database_settings.dk") -> dict:
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

    print(DBHelper.read_settings_file())
    print(DBHelper(**DBHelper.read_settings_file()).check_user("admin3", "23514317"))


if __name__ == '__main__':
    main()
