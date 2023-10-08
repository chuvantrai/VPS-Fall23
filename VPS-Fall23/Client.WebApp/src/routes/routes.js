import config from '@/config';
import Homepage from '@/pages/Homepage';
import Register from '@/pages/Register';
import { HeaderOnly } from '@/layouts';
import Login from '@/pages/Login/index.js';

// route can access without login
const publicRoutes = [
  {
    path: config.routes.homepage,
    component: Homepage,
    layout: HeaderOnly,
  },
  {
    path: config.routes.register,
    component: Register,
    layout: null,
  },
  {
    path: config.routes.login,
    component: Login,
    layout: null,
  }
];

// route need login to access
const privateRoutes = [];

export { publicRoutes, privateRoutes };
