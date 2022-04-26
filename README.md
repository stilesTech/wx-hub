## 一、简介

wx-hub 是一个 微信公众号内容半自动同步工具，可以快速通过微信链接同步其它公众号的文章信息。主要通过微信公众号的素材管理和草稿箱Api
公众号文档：[https://developers.weixin.qq.com/doc/offiaccount/Asset_Management/Adding_Permanent_Assets.html](https://developers.weixin.qq.com/doc/offiaccount/Asset_Management/Adding_Permanent_Assets.html)

wx-operate-api 是wx-hub站点的后台支撑服务，需要添加对应的微信公众号配置才可以正常提供服务。相关配置如下图所示。

![http://stiles.cc/usr/uploads/2022/04/2185062312.png](http://stiles.cc/usr/uploads/2022/04/2185062312.png)

![http://stiles.cc/usr/uploads/2022/04/3081734508.png](http://stiles.cc/usr/uploads/2022/04/3081734508.png)

## 二、注意事项

注意1：两个服务都依赖数据库进行信息交换和传输，具体地址：

注意2：注意修改对应服务的数据库配置账号和密码。

注意3：数据库默认后台登录账号admin,密码123456

## 三、如何部署

wx-hub:是.net core项目，wx-operate-api:是java spring boot项目，都支持容器化部署。

对应步骤参考如下:

wx-hub

```bash
#构建docker image
docker build -t wx-hub:1.0.0 .
#登录docker仓库
docker login ip:port
#推送到对应镜像仓库
docker push ip:port/wx-hub:1.0.0
#docker部署
docker run -d -p 5001:80 --restart=always --name wx-hub wx-hub:1.0.0
```

wx-operate-api

```bash
#需要先修改pom里面的docker仓库地址
mvn dockerfile:build -Pdev
mvn dockerfile:push -Pdev
```

### 四、演示效果

输入地址： http://ip:port 访问,不同公众号需要自己调整公众号配置，具体效果如下：

链接为对应需要同步的公众号文章链接。点击提交后即可同步对应文章内容到新的公众号。

![http://stiles.cc/usr/uploads/2022/04/4157468633.png](http://stiles.cc/usr/uploads/2022/04/4157468633.png)

![http://stiles.cc/usr/uploads/2022/04/1293486863.png](http://stiles.cc/usr/uploads/2022/04/1293486863.png)

## 五、其它问题

同步的图文封面地址怎么修改？

一般情况用默认配置即可，有些特殊情况需要自定义设置，可以做如下配置,公众号的同步的图文的文章地址需要在公众号配置里面配置，如果没有配置默认走后台配置列表里面的default_article_cover的值

是否可以配置多个公众号？

是可以的，不同公众号是需要在对应的微信公众号后台配置相同的配置信息。

wx-hub是怎么指定wx-operate-api地址的？

这个是在后台默认配置列表里面的service_url地址指定的

为何使用两个不同的技术来实现？

个人即兴的开发同步工具，没有投入太多的时间，所以有个人的原因.

其它问题可以联系我进行询问，如回复不及时还请谅解。
