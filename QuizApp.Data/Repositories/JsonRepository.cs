using System.Text.Json;
using QuizApp.Data.Interfaces;

namespace QuizApp.Data.Repositories
{
    public class JsonRepository<T> : IDataRepository<T>
    {
        private readonly string _filePath;

        public JsonRepository(string filePath)
        {
            _filePath = filePath;
        }

        public List<T> LoadData()
        {
            if (!File.Exists(_filePath))
                return new List<T>();

            var jsonData = File.ReadAllText(_filePath);
            return JsonSerializer.Deserialize<List<T>>(jsonData) ?? new List<T>();
        }

        public void SaveData(List<T> data)
        {
            var jsonData = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, jsonData);
        }
    }
}
