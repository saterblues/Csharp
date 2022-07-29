# 系统简介 #

目的：将数据，行为，标识划分成独立的各个部分，方便各个系统解耦，快速进行系统迭代

注意：此系统为ECS系统概念的个人理解，可能和正统的ECS系统构架不一致

---
设想中实现的系统：所有实体可以任意添加组合功能，达到能无限扩展能力的可能。目标的中代码如下演示（仅供参考，非最终实现）：

    Entity dog0 = World.CreateEntity();
    FlySystem.Regist(dog0); //狗0获得了飞翔的能力
    FlySystem.SetFlyMaxSpeed(dog0,10); //设置狗0的最高飞行速度
    Entity dog1 = World.CreateEntity();
    SwimSystem.Regist(dog1); //狗1获得了游泳的能力
    SwimSystem.SetSwimMaxDeep(20); //狗1能潜水20米

---
系统实现主要为4个部分
---

- Component

数据部分，用于描述物体数量，状态等信息的数据体，每个*Component* 实例持有对应的*Entity* 对象，用来表示此*Component* 属于特定的*Entity* 

- System

持有*Component* ，并对*Component* 进行相关的数据操作，统一对*Component* 进行逻辑处理的机构

- Entity

只有唯一标识符，表示是独立的实例，本身不包含任何数据，不进行任何操作

- World

负责管理Entity的生产和销毁操作，负责保存各*System* 实例

---
**系统约束**
---
- Component

只包含*Entity* ，数据值和各种状态值，不拥有任何函数功能

- System

将*System* 分为两种: DataSystem,ActionSystem

DataSystem:存储*Component* 并建立统一的Get Set方法，所有*Component* 的相关操作，都经过对应的DataSystem

ActionSystem:与*Component* 逻辑相关的*System* ，专门负责处理逻辑事务

注意：*System* 只能通过其他*System* 获取或者修改对方*System* 中的*Component* 属性值，不允许直接操作非本系统*Component* 

- Entity

仅仅只有标识符，不持有任何数据

- World

持有所有*System* 实例，*Entity* 实例