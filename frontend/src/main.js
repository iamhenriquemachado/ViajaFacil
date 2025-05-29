import { createApp } from "vue";
import App from "./App.vue";
import { createRouter, createWebHistory } from "vue-router";
import Login from "./components/Login.vue";
import DashboardLayout from "./views/DashboardLayout.vue";
import CreateUser from "./views/CreateUser.vue";
import ListUsers from "./views/ListUsers.vue";
import authService from "./services/authService";
import CreateDestine from "./views/CreateDestine.vue";
import ListDestine from "./views/ListDestine.vue";
import ManageReserve from "./views/ManageReserve.vue";

const routes = [
  {
    path: "/",
    component: Login,
    meta: { requiresAuth: false },
    beforeEnter: async (to, from, next) => {
      const auth = await authService.checkAuthStatus();
      if (auth.isAuthenticated) {
        next("/dashboard");
      } else {
        next();
      }
    },
  },
  {
    path: "/dashboard",
    component: DashboardLayout,
    meta: { requiresAuth: true },
    children: [
      { path: "create-user", component: CreateUser },
      { path: "list-user", component: ListUsers },
      { path: "create-destine", component: CreateDestine },
      { path: "list-destine", component: ListDestine },
      { path: "manage-reserve", component: ManageReserve },
    ],
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Guarda de navegação global
router.beforeEach(async (to, from, next) => {
  // Verificar se a rota requer autenticação
  if (to.matched.some((record) => record.meta.requiresAuth)) {
    // Verificar status de autenticação
    const auth = await authService.checkAuthStatus();
    if (!auth.isAuthenticated) {
      // Redirecionar para login se não estiver autenticado
      next("/");
    } else {
      // Continuar se estiver autenticado
      next();
    }
  } else {
    // Rota não requer autenticação, continuar normalmente
    next();
  }
});

createApp(App).use(router).mount("#app");
