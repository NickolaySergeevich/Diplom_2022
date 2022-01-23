import os

from flask import Flask, jsonify, Response, request

from src.work_with_db.db_helper import DBHelper

# WSGI
application = Flask(__name__)
wsgi_app = application.wsgi_app

# DB
if os.name == "nt":
    db_helper = DBHelper(**DBHelper.read_settings_file("../help_files/database_settings.dk"))
else:
    # Кал говна способ
    db_helper = DBHelper(
        **DBHelper.read_settings_file("/home/std/diplom_2022/api_flask/help_files/database_settings.dk")
    )


@application.route("/api/")
def start_api_page() -> str:
    """
    Стартовая страница
    TODO Добавить описание api

    :return: Пока строка с "Привет, Мир!"
    """

    return "Hello, World!"


@application.route("/api/users/", methods=["GET"])
def get_users() -> Response:
    """
    Получение всех пользователей

    :return: Ответ json
    """

    return jsonify(db_helper.get_users())


@application.route("/api/login/", methods=["GET"])
def login() -> Response:
    """
    Вход в систему

    :return: Ответ json
    """

    username = request.args.get("username")
    password = request.args.get("password")

    answer_from_db = db_helper.login_in(username, password, True)
    if answer_from_db is not None:
        return jsonify(answer_from_db)
    else:
        return jsonify({"status": "404"})


def main() -> None:
    """
    Для прямого запуска

    :return: Ничего
    """

    application.run()


if __name__ == '__main__':
    main()
