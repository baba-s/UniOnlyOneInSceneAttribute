# UniOnlyOneInSceneAttribute

コンポーネントがシーンに1つだけ存在することを保証する Attribute

## 使用例

```cs
using Kogane;
using UnityEngine;

[OnlyOneInScene]
public class Example : MonoBehaviour
{
}
```

OnlyOneInScene 属性を適用したコンポーネントが  
シーンに複数存在する状態で Unity を再生しようとすると  

![2020-09-06_195539](https://user-images.githubusercontent.com/6134875/92324250-e6f95e00-f07a-11ea-808f-302719f9da3c.png)

エラーログが出力されて Unity の再生ができなくなります  
