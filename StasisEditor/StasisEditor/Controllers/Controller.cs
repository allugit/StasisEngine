namespace StasisEditor.Controllers
{
    abstract public class Controller
    {
        public Controller()
        {
            loadResources();
        }

        abstract protected void loadResources();
    }
}
