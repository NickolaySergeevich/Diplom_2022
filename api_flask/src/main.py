from flask import Flask

application = Flask(__name__)
wsgi_app = application.wsgi_app


@application.route("/api/")
def start_api_page() -> str:
    """
    Стартовая страница
    TODO Добавить описание api

    :return: Пока строка с "Привет, Мир!"
    """

    return "Hello, World!"


def main() -> None:
    """
    Для прямого запуска

    :return: Ничего
    """

    application.run()


if __name__ == '__main__':
    main()
