using OpenWeatherMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramBot.Messages;

namespace TelegramBot.Commands
{
    class WeatherCommand : IBotCommand
    {
        public string CommandName => "/weather";

        public CommandResult ProcessInitialMessage(Update inUpdate)
        {
            var message = new TextResponseMessage(inUpdate.Message ?? inUpdate.EditedMessage, "Сообщи, где находишься?");
            return new CommandResult(message, false);
        }

        public CommandResult ProcessMessage(Update inUpdate)
        {
            //ResponseMessage resMes = null;
            //if (inUpdate.Type == Telegram.Bot.Types.Enums.UpdateType.EditedMessage)
            //    resMes = new QuoteTextResponseMessage(inUpdate.EditedMessage, "Не хорошо править старые сообщения");
            //else
            //{
            //    resMes = new TextResponseMessage(inUpdate.Message, $"Echo: {inUpdate.Message.Text.Substring(0, Math.Min(255, inUpdate.Message.Text?.Length ?? 0))}");
            //}
            //return new CommandResult(resMes, false);

            CurrentWeatherResponse weather = null;
            if (inUpdate.Message.Type == Telegram.Bot.Types.Enums.MessageType.TextMessage)
            {
                // try to get weather base on place name
                weather = GetWeatherByName(inUpdate.Message.Text);
            }
            if (inUpdate.Message.Type == Telegram.Bot.Types.Enums.MessageType.LocationMessage)
            {
                weather = GetWeatherByLocation(inUpdate.Message.Location.Longitude, inUpdate.Message.Location.Latitude);
            }

            ResponseMessage resMes = null;
            // Подготовка результата
            if (weather == null)
                resMes = new TextResponseMessage(inUpdate.Message, "Не удалось определить погоду");
            else
                resMes = new TextResponseMessage(inUpdate.Message, weather.ConvertWeatherInText());

            return new CommandResult(resMes, true);
        }


        #region OpenWeatherMap API 
        string APIkey = "9e65c8a3c6962a1cc0a5ec4d7e2c7aa7";
        OpenWeatherMapClient weatherClient = null;

        /// <summary>
        /// Get OpenWeatherMap connection
        /// </summary>
        /// <returns></returns>
        OpenWeatherMapClient GetWeatherServiceClient()
        {
            if (weatherClient == null)
            {
                weatherClient = new OpenWeatherMapClient(APIkey);
            }
            return weatherClient;
        }

        /// <summary>
        /// REquest weather by place name
        /// </summary>
        /// <param name="place"></param>
        /// <returns></returns>
        CurrentWeatherResponse GetWeatherByName(string place)
        {
            try
            {
                var client = GetWeatherServiceClient();
                var resTask = client.CurrentWeather.GetByName(place, MetricSystem.Metric);
                resTask.Wait();
                var res = resTask.Result;
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Request weather by location
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        CurrentWeatherResponse GetWeatherByLocation(float longitude, float latitude)
        {
            try
            {
                var client = GetWeatherServiceClient();
                var resTask = client.CurrentWeather.GetByCoordinates(new Coordinates() { Longitude = longitude, Latitude = latitude }, MetricSystem.Metric);
                resTask.Wait();
                var res = resTask.Result;
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        #endregion
    }

    public static class WeatherExtentionUtils
    {
        /// <summary>
        /// Simple convertation weather response to string
        /// </summary>
        /// <param name="weather"></param>
        /// <returns></returns>
        public static string ConvertWeatherInText(this CurrentWeatherResponse weather)
        {
            return $"Погода в {weather.City.Name}: t°={weather.Temperature.Value}°C (min: {weather.Temperature.Min}°C, max: {weather.Temperature.Max}°C)" +
                $", w={weather.Wind.Speed.Value}м/с ({weather.Wind.Direction.Name})";
        }
    }
}
