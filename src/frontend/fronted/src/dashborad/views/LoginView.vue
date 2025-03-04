<template>
    <div class="container">
        <el-space direction="vertical" :size="18" class="loginBox">
            <h2 class="title">Xinの博客</h2>
            <el-form :model="loginModel" :rules="rules" ref="formRef">
                <el-form-item prop="account">
                    <el-input v-model="loginModel.account" clearable class="input" placeholder="请输入账号">
                        <template #prefix>
                            <el-icon>
                                <UserFilled />
                            </el-icon>
                        </template>
                    </el-input>
                </el-form-item>
                <el-form-item prop="password">
                    <el-input v-model="loginModel.password" clearable show-password class="input" type="password" placeholder="请输入密码">
                        <template #prefix>
                            <el-icon>
                                <Lock />
                            </el-icon>
                        </template>
                    </el-input>
                </el-form-item>
                <el-form-item prop="captcha">
                    <div class="captchaBox">
                        <el-input v-model="loginModel.captcha" clearable class="captcha" placeholder="请输入验证码">
                            <template #prefix>
                                <el-icon>
                                    <CircleCheck />
                                </el-icon>
                            </template>
                        </el-input>
                        <img v-if="captcha.imageSrc" :src="captcha.imageSrc" alt="验证码" class="capathaImage"
                            @click="resetCaptcha" />
                    </div>
                </el-form-item>
            </el-form>
            <div class="login-button" @click="handleLogin">
                登录
            </div>
        </el-space>
    </div>
</template>
<script lang="ts" setup>
import { FormInstance } from 'element-plus'
import { reactive, ref } from 'vue';
import{useRouter} from 'vue-router'
import {SetToken} from '@/common/utils/userStore.js'
import User from "@/dashborad/hooks/user";
const { loginModel, captcha, resetCaptcha, Login } = User();
var rules = reactive({
    account: [
        { required: true, message: '请输入账号', trigger: 'blur' },
        { min: 5, max: 12, message: '长度在5-12位之间', trigger: 'blur' },
    ],
    password: [
        { required: true, message: '请输入密码', trigger: 'blur' },
        { min: 10, max: 18, message: '长度在10-18位之间', trigger: 'blur' },
    ],
    captcha: [
        { required: true, message: '请输入验证码', trigger: 'blur' },
        { min: 4, max: 4, message: '长度不等于4', trigger: 'blur' },
    ],
});
const formRef = ref<FormInstance>();
const router = useRouter();
const handleLogin = () => {
    Login(formRef, (res) => {
        router.push({
            path:"dashborad"
        });
        SetToken(res.data);
    })
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

.captchaBox {
    display: inline-block;
}

.loginBox .captchaBox .captcha {
    height: 52px;
    width: 170px;
    margin-right: 5px;
    vertical-align: middle;
    /* 垂直居中对齐 */
}

.loginBox .captchaBox .capathaImage {
    height: 52px;
    margin-left: 5px;
    vertical-align: middle;
    /* 垂直居中对齐 */
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
