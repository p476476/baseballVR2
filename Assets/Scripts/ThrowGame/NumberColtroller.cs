using UnityEngine;
using System.Collections;

public class NumberColtroller : MonoBehaviour {
    public Transform[] numbers_pos;
    GameObject[] nums;

    Vector3 undergorund = new Vector3( 0, -100, 0 );//放不在場上的牌子的地方

    Material redMat ;
    Material greenMat;

    void Awake()
    {
        nums = new GameObject[10];
    }

    // Use this for initialization
    void Start () {
        

        redMat = Resources.Load("Number/mat_Red", typeof(Material)) as Material;
        greenMat = Resources.Load("Number/mat_Green", typeof(Material)) as Material;

        init();

    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void init()
    {
        for(int i=1;i<10;i++)
        {
            if (nums[i] != null)
            {
                Destroy(nums[i]);
            }
        }
    }

    public void createNumber(int index,int num)
    {
        if(nums[index] !=null)
        {
            Destroy(nums[index],1f);
        }

        nums[index] = Instantiate(Resources.Load("Number/Number"), numbers_pos[index].position, Quaternion.identity) as GameObject;
        nums[index].GetComponent<number>().myNum = num;
        nums[index].GetComponent<number>().myPos = index;
        setColor(index, 0);
       
    }

    public void setNumberText(int num,string text)
    {
        
    }

    public void changeNumber (int index, int num)
    {

    }


    public void destoryNumber(int index,float time)
    {
        Destroy(nums[index], time);
    }

    public void destoryAllNumber(float time)
    {
        for(int i = 1; i < 10; i++)
        {
            if(nums[i]!= null)
                Destroy(nums[i], time);
        }
       
    }
    public void setColor(int num, int color)// color 0 = red; 1 = green 
    {
        if (nums[num] != null)
        {
            if (color == 0)
                nums[num].GetComponent<Renderer>().sharedMaterial = redMat;
            else if (color == 1)
                nums[num].GetComponent<Renderer>().sharedMaterial = greenMat;
        }
        else
        {
            print("number " + num + " is null! u cant change color.");
        }
    }

    public void setGoal(int num)
    {
        print("set goal num" + num + " to color green");
        setColor(num, 1);
    }

    
}
