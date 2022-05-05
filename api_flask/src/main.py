"""Модуль для api всего проекта"""

from __future__ import annotations

import os
from typing import Optional

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
                DBHelper.settings_file = "/home/std/diplom/work_with_db/help_files/database_settings.dk"

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

        Application.get_instance().__application.run(debug=False)

    @staticmethod
    @__application.route("/api/")
    def start_api_page() -> str:
        """
        Стартовая страница
        TODO - Добавить описание api

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

        data = Application.get_data_from_json(("username", "password"), request.get_json())
        if data is None:
            return jsonify({"status": Application.__NO_DATA})

        answer_from_db = DBHelper.get_instance().login_in(**data)
        if answer_from_db is not None:
            return jsonify(answer_from_db)
        else:
            return jsonify({"status": Application.__NO_DATA_IN_DB})

    @staticmethod
    @__application.route("/api/tasks/", methods=["GET"])
    def get_tasks() -> Response:
        """
        Получение списка задач

        :return: Ответ от сервера или ошибка
        """

        return jsonify(DBHelper.get_instance().get_tasks())

    @staticmethod
    @__application.route("/api/chats/", methods=["GET"])
    def get_chat() -> Response:
        """
        Получение чата для пользователей

        :return: Json с чатом
        """

        data = Application.get_data_from_json(("user_from", "user_to", "password"), request.get_json())
        if data is None:
            return jsonify({"status": Application.__NO_DATA})

        if DBHelper.get_instance().login_in(data["user_from"], data["password"]) is not None:
            return jsonify(DBHelper.get_instance().get_chat(data["user_from"], data["user_to"]))
        else:
            return jsonify({"status": Application.__NO_DATA_IN_DB})

    @staticmethod
    @__application.route("/api/registration/", methods=["POST"])
    def registration() -> Response:
        """
        Регистрация нового пользователя

        :return: Json с информацией о том, успешно ли всё прошло
        """

        data = Application.get_data_from_json(("username", "password", "name", "surname"), request.get_json())
        if data is None:
            return jsonify({"status": Application.__NO_DATA})

        return jsonify({"status": DBHelper.get_instance().registration(**data)})

    @staticmethod
    def get_data_from_json(what_need: tuple, request_data: tuple) -> Optional[dict]:
        """
        Получение данных из json

        :param what_need: Какие поля нужны
        :param request_data: То, что отправил пользователь

        :return: Словарь с данными или None
        """

        if request_data is None or not all(key in request_data for key in what_need):
            return None

        return dict([(what_need[i], request_data[what_need[i]]) for i in range(len(what_need))])


def main() -> None:
    """
    Для тестов

    :return: Ничего
    """

    Application.get_instance().start_server()


if __name__ == '__main__':
    main()
