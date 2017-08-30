using System;
using AutoMapper;
using Mobile.Common.Controls.Model;
using Mobile.Common.Services.DataTransfairObject;
using Mobile.Common.Services.Interfaces;
using Xamarin.Forms;

namespace Mobile.Common.Controls.Utils
{
	public class RestServiceManager
	{
		private static IMapper Mappings
		{
			get
			{
				MapperConfiguration config = new MapperConfiguration(cfg =>
				{
					cfg.CreateMap<DtoUserInfo, UserInfo>();
				});

				IMapper mapper = config.CreateMapper();

				return mapper;
			}
		}

		private static IMapper InvertedMappings
		{
			get
			{
				MapperConfiguration config = new MapperConfiguration(cfg =>
				{
					cfg.CreateMap<UserLogin, DtoUserLogin>();
				});

				IMapper mapper = config.CreateMapper();

				return mapper;
			}
		}

		private static IAccountService _accountService;

		private static IAccountService AccountService
		{
			get
			{
				if (_accountService == null)
				{
					_accountService = DependencyService.Get<IAccountService>();

					if (_accountService == null)
					{
						throw new Exception("Missing implementation of " + nameof(IAccountService));
					}
				}

				return _accountService;
			}
		}

		public static UserInfo GetUserInfo(string token)
		{
			DtoUserInfo dtoUserInfo = AccountService.GetUserInfo(token);
			return Mappings.Map<UserInfo>(dtoUserInfo);
		}

		public static void LogoutCurrentUser(string token)
		{
			AccountService.LogoutUser(token);
		}

		public static string LoginUser(UserLogin userLogin)
		{
			DtoUserLogin dtoUserLogin = InvertedMappings.Map<DtoUserLogin>(userLogin);
			return AccountService.LoginUser(dtoUserLogin);
		}

		public static bool IsUserLoggedIn(string token)
		{
			return AccountService.IsLoggedIn(token);
		}
	}
}
