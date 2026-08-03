using UnityEngine;

public class scientistminiboss : MonoBehaviour
{
    public enum Stato {escaping, catching};
    public Stato StatoAttuale;
    public int HitPoints=600;
    void Start()
    {
        if (parassita.StatoAttuale==parassita.Stato.possessing) 
	{	
		this.StatoAttuale=Stato.escaping;
	} else {
		this.StatoAttuale=Stato.catching;
	}
    }
    
    void Update()
    {
        if (this.HitPoints<=0) 
	{
		yield return new WaitForSeconds(1.5f);
		Destroy(this.gameObject);
	}
	switch (StatoAttuale)
	{
		case Stato. catching:
			
			if (parassita.StatoAttuale==parassita.Stato.possessing)
			{
				this.StatoAttuale=Stato.escaping;
			}
			break;
		case Stato.escaping:
			
			if (parassita.StatoAttuale!=parassita.Stato.possessing)
			{
				this.StatoAttuale=Stato.catching;
			}
			break;
	}
    }
}









