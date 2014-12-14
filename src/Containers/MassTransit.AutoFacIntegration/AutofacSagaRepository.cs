﻿// Copyright 2007-2012 Chris Patterson, Dru Sellers, Travis Smith, et. al.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace MassTransit.AutofacIntegration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Autofac;
    using Pipeline;
    using Saga;


    public class AutofacSagaRepository<TSaga> :
        ISagaRepository<TSaga>
        where TSaga : class, ISaga
    {
        readonly string _name;
        readonly ISagaRepository<TSaga> _repository;
        readonly ILifetimeScope _scope;

        public AutofacSagaRepository(ISagaRepository<TSaga> repository, ILifetimeScope scope, string name)
        {
            _repository = repository;
            _scope = scope;
            _name = name;
        }

//        public IEnumerable<Action<IConsumeContext<TMessage>>> GetSaga<TMessage>(IConsumeContext<TMessage> context,
//            Guid sagaId, InstanceHandlerSelector<TSaga, TMessage> selector, ISagaPolicy<TSaga, TMessage> policy)
//            where TMessage : class
//        {
//            return _repository.GetSaga(context, sagaId, selector, policy)
//                              .Select(consumer => (Action<IConsumeContext<TMessage>>)(x =>
//                                  {
//                                      using (_scope.BeginLifetimeScope(_name))
//                                      {
//                                          consumer(x);
//                                      }
//                                  }));
//        }

        public Task Send<T>(ConsumeContext<T> context, IPipe<SagaConsumeContext<TSaga, T>> next) where T : class
        {
            return _repository.Send(context, next);
        }

        public IEnumerable<Guid> Find(ISagaFilter<TSaga> filter)
        {
            return _repository.Find(filter);
        }

        public IEnumerable<TSaga> Where(ISagaFilter<TSaga> filter)
        {
            return _repository.Where(filter);
        }

        public IEnumerable<TResult> Where<TResult>(ISagaFilter<TSaga> filter, Func<TSaga, TResult> transformer)
        {
            return _repository.Where(filter, transformer);
        }

        public IEnumerable<TResult> Select<TResult>(Func<TSaga, TResult> transformer)
        {
            return _repository.Select(transformer);
        }
    }
}