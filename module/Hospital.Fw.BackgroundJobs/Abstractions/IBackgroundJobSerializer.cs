namespace Hospital.Fw.BackgroundJobs.Abstractions;

public interface IBackgroundJobSerializer
{
    string Serialize(object obj);

    object Deserialize(string value, Type type);
}