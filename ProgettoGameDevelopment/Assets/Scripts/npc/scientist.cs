using UnityEngine;

public class scientist : MonoBehaviour
{
	public enum Stato { escaping, catching, possessed};
	public Stato StatoAttuale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (parassita.StatoAttuale==parassita.Stato.possessing) 
		{
		this.StatoAttuale==Stato.escaping;
		} else 
	{
		this.StatoAttuale==Stato.catching;	
		}
    }

    // Update is called once per frame
    void Update()
    {
        switch  (this.StatoAttuale)
	    {
		case Stato.catching:
			
			if (parassita.StatoAttuale==parassita.Stato.Possessing && hit?)
			{
				this.StatoAttuale==Stato.possessed;
				} else
			{
				this.StatoAttuale==Stato.escaping;
				}
		break;
		case Stato.escaping:
		    
			this.StatoAttuale==Stato.catching;
		break;
		case Stato.possessed:
		
			death;
		break;
		    }
    }
}


























