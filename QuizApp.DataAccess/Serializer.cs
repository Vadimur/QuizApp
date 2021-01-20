using System;
using System.IO;
using System.Security;
using System.Text.Json;
using QuizApp.DataAccess.Exceptions;

namespace QuizApp.DataAccess
{
    public class Serializer
    {
        public void Serialize<T>(string filePath, T @object)
        {
            string json = JsonSerializer.Serialize(@object);
            try
            {
                File.WriteAllText(filePath, json);
            }
            catch (Exception exception)
            {
                if (exception is UnauthorizedAccessException ||
                    exception is SecurityException ||
                    exception is IOException)
                {
                    throw new SerializerException(exception.Message, exception);
                }

                throw;
            }
        }

        public T Deserialize<T>(string filePath)
        {
            string json;
            try
            {
                json = File.ReadAllText(filePath);
            }
            catch (Exception exception)
            {
                if (exception is FileNotFoundException)
                {
                    json = "[]";
                    File.WriteAllText(filePath, json);
                    
                }
                else if (exception is UnauthorizedAccessException ||
                    exception is SecurityException ||
                    exception is IOException)
                {
                    throw new SerializerException(exception.Message, exception);
                }
                else
                    throw;
            }

            try
            {
                return JsonSerializer.Deserialize<T>(json);
            }
            catch (Exception exception)
            {
                if (exception is ArgumentNullException ||
                    exception is JsonException)
                {
                    throw new SerializerException(exception.Message, exception);
                }

                throw;
            }
        }
    }
}