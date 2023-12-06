

using System.Collections.Generic;
using Unity.VisualScripting;

public interface IManagedItem<Key>
{
    void Register();
    void Unregister();
}
