import { reactive, ref, inject, onMounted, computed } from 'vue';
import HttpClient from '@/common/utils/httpclient';
import { ElMessage } from 'element-plus';
const loginModel = reactive({
    account: null,
    password: null,
    captcha: null
})
const captcha = reactive({
    imageSrc: null,
    id: null
})
const User = () => {
    const httpclient = inject("httpclient")
    onMounted(async () => {
        var res = await httpclient.get("/api/Login/Captcha");
        captcha.id = res.data.id;
        // 设置 imageSrc 为 data URI 格式
        captcha.imageSrc = res.data.imgSrc;
    });
    async function resetCaptcha() {
        var res = await httpclient.get("/api/Login/Captcha");
        captcha.id = res.data.id;
        // 设置 imageSrc 为 data URI 格式
        captcha.imageSrc = res.data.imgSrc;
    }
    async function Login(formref, onSuccess) {
        formref.value.validate(async (valid) => {
            if (!valid)
                ElMessage.error("表单格式错误");
            else {
                let res = await httpclient.post(`/api/Login/Login?captchaId=${captcha.id}&captchaCode=${loginModel.captcha}`,{
                    account:loginModel.account,
                    password:loginModel.password
                });
                if (!res.success) {
                    ElMessage.error(`${res.message}`);
                    loginModel.captcha = null;
                    resetCaptcha();
                }
                else {
                    if (onSuccess && typeof onSuccess === 'function') {
                        onSuccess(res); // 可以将响应传给 onSuccess 函数
                    };
                }
            }
        });
    }
    return { loginModel, captcha, resetCaptcha, Login }
}
export default User;


