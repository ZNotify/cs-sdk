namespace ZNotify.Utils;

public abstract class Result<TSuccess,TError>
{
    public class Ok:Result<TSuccess,TError>
    {
        public TSuccess Data{get;}
        public Ok(TSuccess data)=>Data=data;
        public void Deconstruct(out TSuccess data)=>data=Data;
    }

    public class Fail:Result<TSuccess,TError>
    {
        public TError Error{get;}
        public Fail(TError error)=>Error=error;
        public void Deconstruct(out TError error)=>error=Error;
    }

    //Convenience methods
    public static Result<TSuccess,TError> Good(TSuccess data)=>new  Ok(data);
    public static Result<TSuccess,TError> Bad(TError error)=>new  Fail(error);
}