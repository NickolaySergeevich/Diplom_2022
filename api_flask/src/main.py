from flask import Flask, jsonify, Response, request

from work_with_db.db_helper import DBHelper

# WSGI
application = Flask(__name__)
wsgi_app = application.wsgi_app

# # DB
# db_helper = DBHelper(**DBHelper.read_settings_file())


@application.route("/")
def start_api_page() -> str:
    """
    Стартовая страница
    TODO Добавить описание api

    :return: Пока строка с "Привет, Мир!"
    """

    return "Hello, World!"


# @application.route("/api/users/", methods=["GET"])
# def get_users() -> Response:
#     return jsonify(db_helper.get_users())
#
#
# @application.route("/api/check_user/", methods=["GET"])
# def check_user() -> Response:
#     return jsonify(
#         [
#             {
#                 "answer": db_helper.check_user(
#                     request.args.get("username"), request.args.get("password"), bool(int(request.args.get("is_md5")))
#                 )
#             }
#         ]
#     )


def main() -> None:
    """
    Для прямого запуска

    :return: Ничего
    """

    application.run()


if __name__ == '__main__':
    main()
