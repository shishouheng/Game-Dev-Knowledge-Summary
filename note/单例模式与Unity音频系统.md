# 单例模式与Unity音频系统

## 一、单例模式

### 1、定义：

单例模式是一种常用的设计模式，它提供了一种创建对象的最佳方式，确保一个类只有一个实例，并提供一个全局访问点 来访问这个唯一的实例。

### 2、优缺点分析：

在游戏开发的过程中，有些类需要频繁的创建对象，这对于一些大型的对象（如AudioManager、GameManager等）来说会造成很大的性能开销，通过使用单例模式可以保证他们只存在唯一一个实例，就降低了内存的使用频率，减轻了GC的压力

但同样也存在一些缺点，一是使用单例模式会增加代码的耦合度和复杂度，二是由于单例对象可以在任何地方访问，追踪和调试可能会比较困难；三是在多线程环境下存在线程安全问题，有可能引发竞态条件和数据不一致的问题

### 3、单例模式的实现

实现单例模式至少需要以下三个步骤：

- 构造函数私有化，防止外部代码通过new关键字创建类的实例

- 定义一个静态变量来存储唯一的实例

- 在类中定义一个公共的静态方法或者属性用于获取唯一的实例。在该方法/属性中先检查静态变量是否已经保持了一个实例。如果没有，则创建一个新的实例并保存在静态变量中。然后返回静态变量中保存的实例

代码如下：

   ```c#
    public class Singleton
    {
        private static Singleton instance;
        
        private Singleton(){}
        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }
    }
```

### 4、Unity中实现单例模式

在Unity中所有脚本都默认继承自Monobehaviour这个类，而继承了Monobehaviour类就意味着这个类可以充当组件添加到游戏物体身上并且可以调用unity提供的生命周期函数来初始化数据、更新数据和清理资源。

继承了Monobehaviour的类无法通过new关键字来创建对象，只能通过将其挂载到游戏物体身上来创建对象，每当一个物体身上挂载了一个继承了Monobehaviour类的脚本，就相当于创建了一个对象。

但由于单例模式中需要通过new对象来创建唯一的实例并存储在一个静态变量中。所以这里需要改变一下思路：判断当前单例脚本挂载到了多少物体身上（也就是实例化了多少次），如果只有一次，那就实现了单例模式，如果大于1，那就给控制台输出一个错误信息，如果0次，那么挂载到某个游戏物体身上。

代码如下：

  ```c#
    public class MonoSingle : MonoBehaviour 
    {
        private static MonoSingle instance;
        public static MonoSingle Instance
        {
            get
            {
               //判断instance是否保存了实例，保存了则直接返回instance，否则进行下一步判断
               if (instance == null)
                {
               //通过FindObjectsOfType将场景中所有挂载MonoSingle的物体数量存在length中
                    int number = FindObjectsOfType<MonoSingle>().Length;
               //大于1说明有多个实例，通过控制台输出错误信息
                    if(number>1)
                    {
                        Debug.LogError("instance count>1");
                    }
               //将挂载了MonoSingle的组件赋给instance
                    instance = FindObjectOfType<MonoSingle>();
               //如果为空说明没有物体挂载MonoSingle组件，则自己创建一个挂载了MonoSingle组件的对象
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(MonoSingle).ToString();
                        instance = obj.AddComponent<MonoSingle>();
                    }
                }
                return instance;
            }
        }
    }
```

**注：`FindObjectsOfType()<MonoSingle>()` 返回的是挂载了MonoSingle组件的所有元素的数组，而`FindObjectOfType()<MonoSingle>` 返回的是挂载了MonoSingle的某个物体**

继承了Monobehaviour的单例模板

   ```c#
    public class SingleTemplate<T> : MonoBehaviour where T:MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    int number = FindObjectsOfType<T>().Length;
                    if (number > 1)
                    {
                        Debug.LogError("instance count>1");
                    }
                    instance = FindObjectOfType<T>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).ToString();
                        instance = obj.AddComponent<T>();
                    }
                }
                return instance;
            }
        }
    }
```

## 二、Unity音频系统

### 1、音频片段的分类

背景音乐：适用于较长的音乐，在游戏过程中会持续播放，一般是MP3、ogg等格式

音效：适用于较短的音乐，发生特定行为时播放，如开门、走路、跳跃时，一般是aiff、wav等格式

### 2、音频系统常用组件

AudioListener：音频监听器，通常附加到需要使用的摄像机上，用于监听游戏中的声音

AudioSource：声源，一般附加到GameObject上，用于播放声音

### 3、AuidoManager

AudioManager在游戏开发中是用于管理游戏中音频资源和音频播放的单例类，在游戏开发的过程中通常会写好一个单例类的模板，然后直接让需要实现单例的组件去继承该模板即可。

例：

   ```c#
    public class AudioManager : SingleTemplate<AudioManager>
    {
        private AudioSource BackGroundSound;
        void Awake()
        {
            BackGroundSound = GetComponent<AudioSource>();
        }
        public void PlayBgMusic(string name)
        {
            if (!BackGroundSound.isPlaying)
            {
                AudioClip clip = Resources.Load<AudioClip>("AudioClips/" + name);
                BackGroundSound.clip = clip;
                BackGroundSound.Play();
            }
        }
        public void StopBgMusic()
        {
            if (BackGroundSound.isPlaying)
            {
                BackGroundSound.Stop();
            }
        }
        public void PlaySound(string name, Vector3 vector3)
        {
            AudioClip clip = Resources.Load<AudioClip>("AudioClips/" + name);
            AudioSource.PlayClipAtPoint(clip, vector3);
        }
    }
```
