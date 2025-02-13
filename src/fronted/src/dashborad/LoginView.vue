<template>
    <div class="container" la>
        <el-space direction="vertical" :size="18" class="loginBox">
            <h2 class="title">Xinの博客</h2>
            <el-input v-model="loginModel.account" clearable class="input" placeholder="请输入账号" />
            <el-input v-model="loginModel.password" clearable class="input" placeholder="请输入密码" />
            <div class="captchaBox">
                <el-input v-model="loginModel.captcha" clearable class="captcha" placeholder="请输入密码" />
            </div>
            <div class="login-button" @click="LoginCheck">
                登录
            </div>
        </el-space>
    </div>
</template>
<script setup>
import HttpClient from '@/common/utils/httpclient';
import { reactive, ref,inject,onMounted } from 'vue';
const loginModel = reactive({
    account: null,
    password: null,
    captcha:null
})
const captcha = reactive({
    captcha:null,
    id:null
})
const httpclient = inject("httpclient")
onMounted(()=>{
    var res = httpclient.get("/api/Login/Captcha");
    console.log(JSON.stringify(res));
})
//登录
const LoginCheck = () => {

}


</script>

<style>
.container {
    background-image: url('@/assets/background.jpg');
    background-position: center;
    background-size: cover;
    overflow: hidden;
    height: 100vh;
    width: 100vw;
    position: relative;
    display: flex;
    justify-content: center;
    align-items: center;
}

.loginBox {
    background: rgba(255, 255, 255, 0.56);
    width: 400px;
    height: 518px;
    border-radius: 12px;
}

.loginBox .input {
    height: 52px;
    width: 300px;
}
.loginBox .captcha{
    height: 52px;
    width: 170px;
    margin-right: 5;
}
.title {
    font-size: 24px;
    font-weight: bold;
    color: #1F1F1F;
    text-align: center;
    margin-top: 10px;
    text-shadow: 2px 2px 4px rgba(0, 0, 0, 0.2);
    font-family: 'Arial', sans-serif;
    margin-top: 50px;
}

.login-button {
    width: 100px;
    /* 设置宽度 */
    margin: 20px auto;
    /* 居中 */
    text-align: center;
    /* 确保文字居中 */
    padding: 10px;
    background-color: #4CAF50;
    /* 背景色 */
    color: white;
    border-radius: 5px;
    cursor: pointer;
}
</style>
