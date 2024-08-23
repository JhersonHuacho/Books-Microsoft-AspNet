﻿namespace MyFirstApi.Services
{
	public class SingletonService : ISingletonService
	{
		private readonly Guid _serviceId;
		private readonly DateTime _createdAt;

		public SingletonService()
		{
			_serviceId = Guid.NewGuid();
			_createdAt = DateTime.UtcNow;
		}

		public string Name => nameof(SingletonService);

		public string SayHello()
		{
			return $"Hello! I am {Name}. My Id is {_serviceId}. Iwas created at { _createdAt: yyyy-MM-dd HH:mm:ss}.";
		}
	}
}
