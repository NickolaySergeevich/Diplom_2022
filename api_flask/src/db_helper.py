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

        self._cursor.close()
        self._connection.close()

    @staticmethod
    def read_settings_file(filename: str = "../help_files/database_settings.dk") -> dict:
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
    DBHelper(**DBHelper.read_settings_file())


if __name__ == '__main__':
    main()
