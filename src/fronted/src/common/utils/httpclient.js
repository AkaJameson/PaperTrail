// httpclient.js
import axios from 'axios';
import { ElMessage } from 'element-plus'

class HttpClient {
  constructor(baseURL, options = {}) {
    this.defaultOptions = {
      returnNativeData: true,  // 是否返回原始response.data
      needAuth: false,          // 是否需要鉴权头
      isFormData: false,        // 是否是form-data格式
      showError: true,          // 是否显示错误提示
      ignoressl: true,
      ...options
    };
    this.instance = axios.create({
      baseURL,
      timeout: 10000,
      headers: {
        'Content-Type': 'application/json',
      },
      withCredentials: true
    }
    );

    // 请求拦截器
    this.instance.interceptors.request.use(
      config => {
        // 处理form-data格式
        if (this.defaultOptions.isFormData && config.data) {
          const formData = new FormData();
          Object.entries(config.data).forEach(([key, value]) => {
            formData.append(key, value);
          });
          config.data = formData;
          config.headers['Content-Type'] = 'multipart/form-data';
        }

        // 添加鉴权头
        if (this.defaultOptions.needAuth && this.authToken) {
          config.headers.Authorization = `Bearer ${this.authToken}`;
        }

        return config;
      },
      error => Promise.reject(error)
    );

    // 响应拦截器
    this.instance.interceptors.response.use(
      response => {
        const res = {
          data: this.defaultOptions.returnNativeData
            ? response
            : response.data,
          status: response.status,
        };
        return res;
      },
      error => {
        if (this.defaultOptions.showError) {
          let message = error.response?.data?.message || error.message;
          ElMessage.error(`请求失败, ${message}`)
        }
        return Promise.reject(error);
      }
    );
  }

  // 设置认证token
  setAuthToken(token) {
    this.authToken = token;
  }

  // 通用请求方法
  async request(config) {
    return this.instance.request(config);
  }

  // 简化请求方法
  get(url, params, options = {}) {
    return this.instance.get(url, {
      params,
      ...this._mergeOptions(options)
    });
  }

  post(url, data, options = {}) {
    return this.instance.post(url, data, this._mergeOptions(options));
  }

  put(url, data, options = {}) {
    return this.instance.put(url, data, this._mergeOptions(options));
  }

  delete(url, options = {}) {
    return this.instance.delete(url, this._mergeOptions(options));
  }

  // 合并配置选项
  _mergeOptions(options) {
    return {
      ...this.defaultOptions,
      ...options
    };
  }
}

export default HttpClient;