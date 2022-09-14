using Telegram.Bot;
using TelegramBotApp;
using TelegramBotApp.Handlers;

var token = "5679860642:AAHt9I0kp_7s89avcF6aBGnK1yC3OLOE1Dw";

ITelegramBotClient telegramBotClient = new TelegramBotClient(token);
IMessageHandler messageHandler = new MessageHandler(telegramBotClient);
IExceptionHandler exceptionHandler = new ExceptionHandler(telegramBotClient: telegramBotClient);

IApplication application = new TelegramBotApplication(telegramBotClient: telegramBotClient, messageHandler:
    messageHandler, exceptionHandler: exceptionHandler);

application.Run();

Console.Read();