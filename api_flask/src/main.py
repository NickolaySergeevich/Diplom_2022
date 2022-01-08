from flask import Flask, jsonify

from src.work_with_db.db_helper import DBHelper

# WSGI
application = Flask(__name__)
wsgi_app = application.wsgi_app

# DB
db_helper = DBHelper(**DBHelper.read_settings_file())


@application.route("/api/")
def start_api_page() -> str:
    """
    Стартовая страница
    TODO Добавить описание api

    :return: Пока строка с "Привет, Мир!"
    """

    return "Hello, World!"


@application.route("/api/users/", methods=["GET"])
def get_users():
    return jsonify(db_helper.get_users())


def main() -> None:
    """
    Для прямого запуска

    :return: Ничего
    """

    application.run()


if __name__ == '__main__':
    main()
