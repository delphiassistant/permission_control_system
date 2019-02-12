# Permission Control System
This is an SDK which helps you to specify which user group (role) members have access to which actions in controllers. 

## Features:
- Fully integrated with ASP.NET Identity
- Permission Control Feature to protect every Controller/Action.
- Scan/Import new Controller/Actions on next execution of the project.
- Implementation of Users and Roles controllers which is a missing feature in ASP.NET Identity.
- Procedures to Rename ASP.NET Identity related Tables/Columns to match to your requirements (look OnModelCreating method in IdentityModels.cs).
- Implentation of TitleAttribute and IconAttribute to give Controller/Action names pretty titles and icons.
- Implentation of TitleAndIconFilter global filter to automatically read values of Titles and Icons and put them into the ViewBag.
- Implentation of ActionLinkPermission Html Helper to make creation of ActionsLinks easy, based on user permissions.
- Implentation of One-Time-Password login using SMS.

<h1 dir='rtl'> سیستم کنترل دسترسی</h1>
<p dir='rtl'>
این کیت نرم افزاری به شما اجازه می دهد مشخص کنید که کاربران سایت شما بر حسب گروه کاربری یا Role شان به چه Action هایی از چه کنترلر هایی دسترسی داشته باشند.
</p>
<h2 dir='rtl'>قابلیت های این SDK:</h2>
<ul dir='rtl'>
<li>پیاده سازی کامل بر اساس امکانات ASP.NET Identity</li>
<li>امکان کنترل دسترسی برای حفاظت از هر کنترلر/اکشن</li>
<li>اسکن کردن و افزودن تمام کنترلرها/اکشن های جدید در اجرای بعدی برنامه</li>
<li>پیاده سازی کنترلرهای مدیریت Role ها و User ها که یکی از کمبودهای فریم ورک ASP.NET Identity محسوب می شود</li>
<li>شامل پروسه تغییر نام دادن جدول ها و ستون های مورد استفاده ASP.NET Identity جهت تطبیق با سلیقه شما</li>
<li>پیاده سازی TitleAttribute و IconAttribute جهت افزودن عنوان و آیکن های زیبا به کنترلرها و اکشن ها</li>
<li>پیاده سازی فیلتر گلوبال TitleAndIconFilter جهت خواندن مقادیر عنوان ها و آیکن های کنترلرها و اکشن ها و افزودن آنها به ViewBag</li>
<li>پیاده سازی کلاس Html Helper به نام ActionLinkPermission جهت آسان نمودن تولید ActionLink ها بر اساس دسترسی های کاربر</li>
<li>پیاده سازی مکانیسم رمز یک بار مصرف (OTP) با استفاده از ارسال پیامک</li>
</ul>
<h2 dir='rtl'>آموزش ویدئویی کلیه اجزاء با توضیح خط به خط:</h2>
<p dir='rtl'>
همچنین، جهت آن دسته از کاربران که نیاز به آموزش در زمینه نحوه کارکرد این سیستم و نیز آشنایی با نجوه عملکرد ASP.NET Identity دارند یک آموزش ویدئویی کامل تهیه شده که در این آدرس قابل دسترس است و می توانید پس از ثبت نام، از آن استفاده کنید:


</p>
<p dir='rtl'>
<a dir='rtl' href='https://barnamenevis.net/Home/Course/14?کیت-توسعه-نرم-افزار-(SDK)-+-آموزش-ویدئویی-Permission-Control-System-در-ASP.NET-MVC'>کیت توسعه نرم افزار (SDK) + آموزش ویدئویی Permission Control System در ASP.NET MVC</a>
</p>
<p dir='rtl'>در صورت نیاز به تهیه وب سرویس ارسال پیامک جهت استفاده در بخش LoginByOTP، من استفاده از سرویس های
<a href='http://login.parsgreen.com/frame/UserOnlineRegister.aspx?RAUK=WFwOBJnlW2o%3d'>شرکت پارس گرین</a> را توصیه می کنم.
من از سرویس های این شرکت در همه پروژه هایم استفاده کرده ام و رضایت کامل از خدمات پشتیبانی شرکت فوق دارم.
</p>
