using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager  {

    private static ResourceManager s_instance;
    public static ResourceManager instance{
        get {
            if(s_instance == null)
                s_instance = new ResourceManager();
            return s_instance;
        }
    }

    public ResourceManager()
    {
        s_instance = this;
    }




}
