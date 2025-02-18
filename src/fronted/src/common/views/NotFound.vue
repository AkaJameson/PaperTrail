<template>
    <el-result title="404" :sub-title="msg">
        <template #icon>
            <el-image :src="`https://cdn.seovx.com/?mom=${Math.ceil(Math.random() * 1000)}`" />
        </template>
        <template #extra>
            <el-button type="primary">后退</el-button>
        </template>
    </el-result>
</template>
<script setup lang="ts">
const props = defineProps<{
    msg: string
}>()
</script>
