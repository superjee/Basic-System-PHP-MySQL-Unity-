[System.Serializable]
public class ServerResponse<T>
{
    public bool success;
    public string message;
    public T data;
}