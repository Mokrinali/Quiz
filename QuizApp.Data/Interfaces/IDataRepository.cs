namespace QuizApp.Data.Interfaces
{
    public interface IDataRepository<T>
    {
        List<T> LoadData();
        void SaveData(List<T> data);
    }
}
