from __future__ import annotations

import os

from flask import Flask, Response, jsonify, request

from work_with_db.db_helper import DBHelper


class Application:
    """
    Класс для обработки api.
    Использует паттерн singleton
    """

    __instance = None  # Объект в единственном виде
    __application = Flask(__name__)  # Переменная всего приложения Flask
    __wsgi_app = __application.wsgi_app  # Для запуска WSGI

    # Codes
    __NO_DATA = 404
    __NO_DATA_IN_DB = 502

    def __init__(self):
        """
        Конструктор стандартный - создаёт приложение Flask и WSGI
        + Определяет настройки для бд
        """

        if not Application.__instance:
            Application.__instance = self

            # Определение файла настроек для бд
            if os.name == "nt":
                DBHelper.settings_file = "../../work_with_db/help_files/database_settings.dk"
            else:
                DBHelper.settings_file = "/home/std/diplom_2022/api_flask/help_files/database_settings.dk"

    @staticmethod
    def get_instance() -> Application:
        """
        Получение текущего объекта - он может быть только один!

        :return: Объект типа Application
        """

        if not Application.__instance:
            Application()

        return Application.__instance

    @staticmethod
    def start_server() -> None:
        """
        Запускает сервер

        :return: Ничего
        """

        Application.get_instance().__application.run()

    @staticmethod
    @__application.route("/api/")
    def start_api_page() -> str:
        """
        Стартовая страница
        TODO Добавить описание api

        :return: Пока строка с "Привет, Мир!"
        """

        return "Hello, World!"

    @staticmethod
    @__application.route("/api/users/", methods=["GET"])
    def get_users() -> Response:
        """
        Получение имён всех пользователей

        :return: json с именами пользователей
        """

        return jsonify(DBHelper.get_instance().get_users_name())

    @staticmethod
    @__application.route("/api/login/", methods=["GET"])
    def login() -> Response:
        """
        Вход в систему
        Пароль передавать только в md5!

        :return: Ответ либо информация о пользователе, либо ошибка
        """

        what_need = ("username", "password")

        request_data = request.get_json()
        if request_data is None or not all(key in request_data for key in what_need):
            return jsonify({"status": Application.__NO_DATA})

        data = dict([(what_need[i], request_data[what_need[i]]) for i in range(len(what_need))])

        answer_from_db = DBHelper.get_instance().login_in(**data)
        if answer_from_db is not None:
            return jsonify(answer_from_db)
        else:
            return jsonify({"status": Application.__NO_DATA_IN_DB})


# @application.route("/api/login/", methods=["GET"])
# def login() -> Response:
#     """
#     Вход в систему
#     Пароль передавать только в md5
#
#     :return: Ответ json
#     """
#
#     username = request.args.get("username")
#     password = request.args.get("password")
#
#     answer_from_db = db_helper.login_in(username, password, True)
#     if answer_from_db is not None:
#         return jsonify(answer_from_db)
#     else:
#         return jsonify({"status": "404"})
#
#
# @application.route("/api/tasks/", methods=["GET"])
# def get_tasks() -> Response:
#     """
#     Получение списка всех задач
#
#     :returns: Ответ json
#     """
#
#     return jsonify(db_helper.get_tasks())
#
#
# @application.route("/api/chats/", methods=["GET"])
# def get_chat() -> Response:
#     """
#     Получение чата для пользователей
#
#     :return: Ответ json
#     """
#
#     username_from = request.args.get("username_from")
#     password_from = request.args.get("password_from")
#     is_mdfive_password = request.args.get("is_md5")
#     username_to = request.args.get("username_to")
#
#     if is_mdfive_password == '0':
#         is_mdfive_password = False
#     else:
#         is_mdfive_password = True
#
#     if db_helper.login_in(username_from, password_from, is_mdfive_password) is not None:
#         return jsonify(db_helper.get_chat(username_from, username_to))
#     else:
#         return jsonify({"status": "404"})
#
#
# @application.route("/api/registration/", methods=["POST"])
# def registration() -> Response:
#     """
#     Регистрация нового пользователя
#
#     :return: Ответ Json
#     """
#
#     request_data = request.get_json()
#     if request_data is None:
#         return jsonify({"status": "404"})
#     if not all(key in request_data for key in ("username", "password", "name", "surname", "is_mdfive")):
#         return jsonify({"status": False})
#
#     username = request_data["username"]
#     password = request_data["password"]
#     name = request_data["name"]
#     surname = request_data["surname"]
#
#     is_mdfive = bool()
#     try:
#         temp = int(request_data["is_mdfive"])
#         is_mdfive = bool(temp)
#     except ValueError:
#         return jsonify({"status": "422"})
#
#     return jsonify({"status": db_helper.registration(username, password, name, surname, is_mdfive)})


def main() -> None:
    """
    Для тестов

    :return: Ничего
    """

    Application.get_instance().start_server()


if __name__ == '__main__':
    main()
