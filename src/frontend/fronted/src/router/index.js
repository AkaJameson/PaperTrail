import { createRouter, createWebHistory,useRoute } from 'vue-router'
const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: ()=>import('@/dashborad/views/LoginView.vue'),
    },
    {
      path:"/dashborad",
      component:()=>import('@/dashborad/views/Dashborad.vue')
    },
    {
      name:"error",
      path:"/error/:msg?",
      component:()=>import('@/common/views/NotFound.vue'),
      props:true
    }
  ],
  
})

export default router
