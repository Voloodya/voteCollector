using CollectVoters.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using voteCollector.Data;
using voteCollector.Models;

namespace voteCollector.Controllers
{
    public class AccountController : Controller
    {
        private VoterCollectorContext db_context;
        private ServiceUser _serviceUser;
        public AccountController(VoterCollectorContext context)
        {
            db_context = context;
            _serviceUser = new ServiceUser(context);
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                User user = await db_context.User
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync(u => u.UserName == model.UserName && u.Password == model.Password);
                if (user != null)
                {
                    await Authenticate(user); // аутентификация

                    // Загрузка групп авторизовавшегося пользователя
                    List<Groupu> groupsUser = _serviceUser.GetGroupsUser(user.UserName);
                    // Получение группы "Волонтеры"
                    Groupu volunteerGroup = db_context.Groupu.Where(g => g.Name.Equals("Волонтеры")).FirstOrDefault();

                    // Если пользователь состоит только в группе "Волонтеры"
                    if (groupsUser.Count == 1 && volunteerGroup != null && groupsUser.Contains(volunteerGroup))
                    {
                        return RedirectToAction("Index", "QRcode");
                    }
                    // проверяем, принадлежит ли URL приложению
                    else if(!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }                    
                    else
                    {
                        return RedirectToAction("Index", "Friends");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный логин и (или) пароль");
                }
            }
            return View(model);
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = await db_context.User.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (user == null)
                {
                    // добавляем пользователя в бд
                    user = new User { UserName = model.UserName, Password = model.Password, Name = model.Name, FamilyName = model.FamilyName, Telephone = model.Telephone };
                    //При регистрации пользователю будет присваиваться роль "user", которая, как ожидается,
                    //добавляется в базу данных с помощью инициализации в классе Startup.
                    Role userRole = await db_context.Role.FirstOrDefaultAsync(r => r.Name == "user");
                    if (userRole != null)
                        user.Role = userRole;

                    db_context.User.Add(user);
                    await db_context.SaveChangesAsync();

                    await Authenticate(user); // аутентификация

                    return RedirectToAction("Login", "Account");
                }
                else
                    ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            }
            return View(model);
        }

        private async Task Authenticate(User user)
        {
            // создаем один claim
            //Объекты claim представляют некоторую информацию о пользователе, которую мы можем использовать для авторизации в приложении.
            //у пользователя может быть определенный возраст, город, страна проживания, любимая музыкальная группа и прочие признаки.
            // И все эти признаки могут представлять отдельные объекты claim. И в зависимости от значения этих claim мы можем предоставлять пользователю доступ к тому или иному ресурсу.
            //Таким образом, claims представляют более общий механизм авторизации нежели стандартные логины или роли, которые привязаны лишь к одному определенному признаку пользователя.
            //Каждый объект claim представляет класс Claim, который определяет следующие свойства:
            //Issuer: "издатель" или название системы, которая выдала данный claim
            //Subject: возвращает информацию о пользователе в виде объекта ClaimsIdentity
            //Type: возвращает тип объекта claim
            //Value: возвращает значение объекта claim
            //Для правильного создания и настройки объекта ClaimsPrincipal вначале создается список claims - набор данных,
            //которые шифруются и добавляются в аутентификационные куки.
            //Каждый такой claim принимает тип и значение.В нашем случае у нас только один claim,
            //который в качестве типа принимает константу ClaimsIdentity.DefaultNameClaimType, а в качестве значения -email пользователя.
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.UserName),
                // Усанавливается роль, которой обладает пользователь
                // Для указания роли здесь применяется тип claim ClaimsIdentity.DefaultRoleClaimType
                new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role?.Name)
            };
            // создаем объект ClaimsIdentity
            //С помощью объекта ClaimsIdentity, который возвращается свойством User.Identity,
            //мы можем управлять объектами claim у текущего пользователя
            ClaimsIdentity id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            //app.UseCookieAuthentication - схема аутентификации, которая была использована при установки middleware app.UseCookieAuthentication в классе Startup
            // в качестве второго параметра передается объект ClaimsPrincipal, который представляет пользователя
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
            //И после вызова метода расширения HttpContext.SignInAsync в ответ клиенту будут отправляться аутентификационные куки,
            //которые при последующих запросах будут передаваться обратно на сервер, десериализоваться и использоваться для аутентификации пользователя.
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
