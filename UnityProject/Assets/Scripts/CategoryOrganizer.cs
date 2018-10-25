using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoryOrganizer : MonoBehaviour {

    public List<List<GameObject>> categories= new List<List<GameObject>>() ;

    public List<GameObject> cat1 = new List<GameObject>();
    public List<GameObject> cat2 = new List<GameObject>();
    public List<GameObject> cat3 = new List<GameObject>();
    public List<GameObject> cat4 = new List<GameObject>();
    public List<GameObject> cat5 = new List<GameObject>();


	// Use this for initialization
	void Awake () {
        categories.Add(cat1);
        categories.Add(cat2);
        categories.Add(cat3);
        categories.Add(cat4);
        categories.Add(cat5);
	}

    public void listsShuffle(){
        for (int i = 0; i < categories.Count;i++){
            for (int b = 0; b < categories[i].Count; b++)
            {
                var temp = categories[i][b];
                int randomIndex = Random.Range(b, categories[i].Count);
                categories[i][b] = categories[i][randomIndex];
                categories[i][randomIndex] = temp;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
