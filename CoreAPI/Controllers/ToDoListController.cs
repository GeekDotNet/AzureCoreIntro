using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CoreAPI.Controllers
{
    
    public class ToDo
    {
        private static int taskId = 0;
        //public Guid TaskId { get; set; } = Guid.NewGuid();
        public int TaskId { get; set; } = GetTaskId();
        public string TaskName { get; set; }
        public DateTime TaskStartDateTime { get; set; }
        public DateTime TaskEndDateTime { get; set; }
        public Enum Priority { get; set; }
        public DateTime CacheDateTime { get; set; }

        private static int GetTaskId()
        {
            return ++taskId;
        }
    }

    enum Priority
    {
        LOW,
        MEDIUM,
        HIGH

    }

    [Route("api/[controller]")]
    public class ToDoListController : Controller
    {
        private IMemoryCache iCache;
        private static readonly string ToDoCacheKey = "ToDoCacheKey";
        private List<ToDo> TodoCache = new List<ToDo>();

        public ToDoListController(IMemoryCache cache)
        {
            iCache = cache;
        }
        // GET api/GetTasks
        [HttpGet]
        public IEnumerable<ToDo> GetTasks()
        {
            if (!iCache.TryGetValue(ToDoCacheKey, out List<ToDo> cacheEntry))
            {
                AddTasks(DateTime.Now);

            }
            else
            {
                cacheEntry.Add(new ToDo
                {
                    TaskName = "Project Cache",
                    Priority = Priority.HIGH,
                    TaskStartDateTime = new DateTime(2018, 07, 30, 13, 30, 00),
                    TaskEndDateTime = new DateTime(2018, 07, 30, 14, 30, 00),
                    CacheDateTime = DateTime.Now
                });
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1).SetSlidingExpiration(TimeSpan.FromMinutes(10));
                iCache.Set(ToDoCacheKey, cacheEntry);
            }
            return iCache.Get(ToDoCacheKey) as List<ToDo>;
       
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]List<ToDo> addTask)
        {

            if (!iCache.TryGetValue(ToDoCacheKey, out List<ToDo> todoCache))
            {
                AddTasks(DateTime.Now);
            }
            else
            {
                todoCache.Add(new ToDo
                {
                    TaskName = "Project Cache",
                    Priority = Priority.HIGH,
                    TaskStartDateTime = new DateTime(2018, 07, 30, 13, 30, 00),
                    TaskEndDateTime = new DateTime(2018, 07, 30, 14, 30, 00),
                    CacheDateTime = DateTime.Now
                });
            }
        }

        private void AddTasks(DateTime dt)
        {

            TodoCache.Add(new ToDo
            {
                TaskName = "Project Azure",
                Priority = Priority.HIGH,
                TaskStartDateTime = new DateTime(2018, 07, 30, 8, 30, 00),
                TaskEndDateTime = new DateTime(2018, 07, 30, 9, 30, 00),
                CacheDateTime = dt
            });

            TodoCache.Add(new ToDo
            {
                TaskName = "Project Web API",
                Priority = Priority.MEDIUM,
                TaskStartDateTime = new DateTime(2018, 07, 30, 10, 00, 00),
                TaskEndDateTime = new DateTime(2018, 07, 30, 10, 30, 00),
                CacheDateTime = dt
            });

            TodoCache.Add(new ToDo
            {
                TaskName = "Project SQL",
                Priority = Priority.LOW,
                TaskStartDateTime = new DateTime(2018, 07, 30, 11, 00, 00),
                TaskEndDateTime = new DateTime(2018, 07, 30, 11, 30, 00),
                CacheDateTime = dt
            });
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetSize(1).SetSlidingExpiration(TimeSpan.FromMinutes(10));
            iCache.Set(ToDoCacheKey, TodoCache);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/ToDoList
        [HttpDelete]
        public void Delete()
        {

            if (iCache.TryGetValue(ToDoCacheKey, out List<ToDo> cacheKey))
            {
                iCache.Remove(ToDoCacheKey);
            }
            
        }
    }
}
