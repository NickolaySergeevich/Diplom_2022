"""Модуль для api всего проекта"""

from __future__ import annotations

import os
from typing import Optional

from flask import Flask, Response, jsonify, request
from werkzeug.datastructures import MultiDict

from work_with_db.db_helper import DBHelper

# Codes
NO_DATA = 404
NO_DATA_IN_DB = 502

application = Flask(__name__)  # Переменная всего приложения Flask

if os.name == "nt":
    DBHelper.settings_file = "../../work_with_db/help_files/database_settings.dk"
else:
    DBHelper.settings_file = "/home/std/diplom/work_with_db/help_files/database_settings.dk"


@application.route("/api/")
def start_api_page() -> str:
    """
    Стартовая страница
    TODO - Добавить описание api

    :return: Пока строка с "Привет, Мир!"
    """

    return "Hello, World!"


@application.route("/api/users/", methods=["GET"])
def get_users() -> Response:
    """
    Получение имён всех пользователей

    :return: json с именами пользователей
    """

    return jsonify(DBHelper.get_instance().get_users_name())


@application.route("/api/get_user_by_name/", methods=["GET"])
def get_user_by_name() -> Response:
    """
    Получение id пользователя по имени

    :return: json с id пользователя или ошибка
    """

    data = get_data_from_args(("username",), request.args)
    if data is None:
        return jsonify({"status": NO_DATA})

    answer_from_db = DBHelper.get_instance().get_user_id(**data)
    if answer_from_db is not None:
        return jsonify(answer_from_db)
    else:
        return jsonify({"status": NO_DATA_IN_DB})


@application.route("/api/login/", methods=["GET"])
def login() -> Response:
    """
    Вход в систему
    Пароль передавать только в md5!

    :return: Ответ либо информация о пользователе, либо ошибка
    """

    data = get_data_from_args(("username", "password"), request.args)
    if data is None:
        return jsonify({"status": NO_DATA})

    answer_from_db = DBHelper.get_instance().login_in(**data)
    if answer_from_db is not None:
        return jsonify(answer_from_db)
    else:
        return jsonify({"status": NO_DATA_IN_DB})


@application.route("/api/tasks/", methods=["GET"])
def get_tasks() -> Response:
    """
    Получение списка задач

    :return: Ответ от сервера или ошибка
    """

    return jsonify(DBHelper.get_instance().get_tasks())


@application.route("/api/chats/", methods=["GET"])
def get_chat() -> Response:
    """
    Получение чата для пользователей

    :return: Json с чатом
    """

    data = get_data_from_json(("user_from", "user_to", "password"), request.get_json())
    if data is None:
        return jsonify({"status": NO_DATA})

    if DBHelper.get_instance().login_in(data["user_from"], data["password"]) is not None:
        return jsonify(DBHelper.get_instance().get_chat(data["user_from"], data["user_to"]))
    else:
        return jsonify({"status": NO_DATA_IN_DB})


@application.route("/api/registration/", methods=["POST"])
def registration() -> Response:
    """
    Регистрация нового пользователя

    :return: Json с информацией о том, успешно ли всё прошло
    """

    data = get_data_from_json(("username", "password", "name", "surname"), request.get_json())
    if data is None:
        return jsonify({"status": NO_DATA})

    return jsonify({"status": DBHelper.get_instance().registration(**data)})


@application.route("/api/tasks/sign_up_to_task/", methods=["POST"])
def sign_up_to_task() -> Response:
    """
    Регистрация на конкурс

    :return: Json с информацией о том, успешно ли всё прошло
    """

    data = get_data_from_json(("users_id", "task_id"), request.get_json())
    if data is None:
        return jsonify({"status": NO_DATA})

    return jsonify({"status": DBHelper.get_instance().sign_up_to_task(**data)})


@application.route("/api/tasks/remove_from_task/", methods=["POST"])
def remove_from_task() -> Response:
    """
    Удаление с конкурса

    :return: Json с информацией о том, успешно ли всё прошло
    """

    data = get_data_from_json(("users_id", "task_id"), request.get_json())
    if data is None:
        return jsonify({"status": NO_DATA})

    return jsonify({"status": DBHelper.get_instance().remove_from_task(**data)})


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


def get_data_from_args(what_need: tuple, args: MultiDict[str, str]) -> Optional[dict]:
    """
    Получение словаря с данными из аргументов пользователя

    :param what_need: Какие поля нужны
    :param args: Аргументы запроса

    :return: Словарь с данными или None
    """

    if args is None or not all(key in args.to_dict() for key in what_need):
        return None

    return args.to_dict()


def main() -> None:
    """
    Для тестов

    :return: Ничего
    """

    application.run()


if __name__ == '__main__':
    main()
