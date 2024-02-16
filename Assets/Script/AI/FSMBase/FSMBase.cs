using UnityEngine;

public abstract class FSMBase : MonoBehaviour
{
    private StateBase currentState = null;
    
    
    protected void ChangeState<T>() where T : new()
    {
        if(currentState != null) currentState.DestroyState();
        currentState = new T() as StateBase;
        if (!currentState) return;
        currentState.InitState(this);
    }
    #region Behaviors
    protected virtual void InitFSM()
    {
        
    }
    protected virtual void UpdateFSM()
    {
        if (!currentState) return;
        currentState.UpdateState(Time.deltaTime);
    }
    protected virtual void DestroyFSM()
    {
        if (!currentState) return;
        currentState.DestroyState();
    }
    #endregion
    #region Unity Methods
    private void Start()
    {
        InitFSM();
        if(currentState)
            Debug.Log("ok");
            
    }
    private void Update()
    {
        UpdateFSM();
    }
    private void OnDestroy()
    {
        DestroyFSM();
    }
    #endregion
}
