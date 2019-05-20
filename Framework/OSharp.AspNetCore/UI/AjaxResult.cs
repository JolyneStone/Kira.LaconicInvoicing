using OSharp.Data;
using System;
using System.Threading.Tasks;

namespace OSharp.AspNetCore.UI
{
    /// <summary>
    /// 表示Ajax操作结果 
    /// </summary>
    public class AjaxResult
    {
        /// <summary>
        /// 初始化一个<see cref="AjaxResult"/>类型的新实例
        /// </summary>
        public AjaxResult()
            : this(null)
        { }

        /// <summary>
        /// 初始化一个<see cref="AjaxResult"/>类型的新实例
        /// </summary>
        public AjaxResult(string content, AjaxResultType type = AjaxResultType.Success, object data = null)
            : this(content, data, type)
        { }

        /// <summary>
        /// 初始化一个<see cref="AjaxResult"/>类型的新实例
        /// </summary>
        public AjaxResult(string content, object data, AjaxResultType type = AjaxResultType.Success)
        {
            Type = type;
            Content = content;
            Data = data;
        }

        /// <summary>
        /// 获取或设置 Ajax操作结果类型
        /// </summary>
        public AjaxResultType Type { get; set; }

        /// <summary>
        /// 获取或设置 消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置 返回数据
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccessed()
        {
            return Type == AjaxResultType.Success;
        }

        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError()
        {
            return Type == AjaxResultType.Error;
        }

        /// <summary>
        /// 成功的AjaxResult
        /// </summary>
        public AjaxResult Success(object data = null)
        {
            this.Type = AjaxResultType.Success;
            this.Content = "操作执行成功";
            this.Data = data;
            return this;
        }

        /// <summary>
        /// 失败的AjaxResult
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public AjaxResult Error(string content = null)
        {
            this.Type = AjaxResultType.Error;
            this.Content = content;
            return this;
        }

        /// <summary>
        /// 成功的AjaxResult
        /// </summary>
        public static AjaxResult CreateSuccess(object data = null)
        {
            return new AjaxResult("操作执行成功", AjaxResultType.Success, data);
        }

        /// <summary>
        /// 失败的AjaxResult
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static AjaxResult CreateError(string content = null)
        {
            return new AjaxResult(content, AjaxResultType.Error);
        }

        /// <summary>
        /// 执行业务方法
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public static AjaxResult Business(Func<AjaxResult> business)
        {
            try
            {
                return business();
            }
            catch (Exception ex)
            {
                return new AjaxResult(ex.GetBaseException().Message, AjaxResultType.Error);
            }
        }

        /// <summary>
        /// 执行业务方法
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public static async Task<AjaxResult> Business(Func<Task<AjaxResult>> business)
        {
            try
            {
                return await business();
            }
            catch (Exception ex)
            {
                return new AjaxResult(ex.GetBaseException().Message, AjaxResultType.Error);
            }
        }

        /// <summary>
        /// 执行业务方法
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public static async Task<AjaxResult> Business(Func<AjaxResult, Task> business)
        {
            var result = new AjaxResult();
            try
            {
                await business(result);
                return result;
            }
            catch (Exception ex)
            {
                return new AjaxResult(ex.GetBaseException().Message, AjaxResultType.Error);
            }
        }

        /// <summary>
        /// 执行业务方法
        /// </summary>
        /// <param name="business"></param>
        /// <returns></returns>
        public static AjaxResult Business(Action<AjaxResult> business)
        {
            var result = new AjaxResult();
            try
            {
                business(result);
                return result;
            }
            catch (Exception ex)
            {
                return new AjaxResult(ex.GetBaseException().Message, AjaxResultType.Error);
            }
        }
    }
}