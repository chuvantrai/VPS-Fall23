import Header from '@/layouts/components/Header';
import AccountProfile from '@/pages/AccountProfile';
import ViewRequestedParkingZone from '@/pages/ViewRequestedParkingZones';
import ManagerLayout from '@/layouts/ManagerLayout';
import guidGenerator from '@/helpers/guidGenerator';
import ViewListParkingZoneOwner from '@/pages/Homepage/components/Content/ViewListParkingZoneOwner';
import AdminOverview from '@/pages/Dashboard/AdminOverview';
import ViewListParkingZone from '@/pages/Homepage/components/Content/ListParkingZone';
import ListUserReport from '../pages/ListUserReport';
import ListAddressManager from '../pages/ListAddressManager/ListAddressManager.jsx';
import ChangePassword from "@/pages/ChangePassword/index.js";

export const adminRoutesConfig = {
  header: Header,
  footer: null,
  layout: ManagerLayout,
  routes: [
    {
      key: guidGenerator(),
      path: '',
      label: 'Trang chủ',
      component: AdminOverview,
      description: '',
    },
    {
      key: guidGenerator(),
      path: 'user',
      label: 'Người dùng',
      children: [
        {
          key: guidGenerator(),
          path: 'profile',
          label: 'Thông tin cá nhân',
          component: AccountProfile,
          description: 'Mọi thông tin về tài khoản được hiển thị dưới đây',
        }
      ],
    },
    {
      key: guidGenerator(),
      path: 'parking-zone',
      label: 'Bãi đỗ xe',
      children: [
        {
          key: guidGenerator(),
          path: '',
          label: 'Danh sách bãi đỗ xe',
          description: 'Toàn bộ danh sách bãi gửi xe hiển thị dưới đây',
          component: ViewListParkingZone,
        },
        {
          key: guidGenerator(),
          path: 'parking-zone-owner',
          label: 'Danh sách chủ bãi gửi xe',
          description: 'Toàn bộ danh sách chủ bãi gửi xe hiển thị dưới đây',
          component: ViewListParkingZoneOwner,
        },
        {
          key: guidGenerator(),
          path: 'requested-list',
          label: 'Danh sách yêu cầu',
          description: 'Toàn bộ danh sách yêu cầu bãi gửi xe hiển thị dưới đây',
          component: ViewRequestedParkingZone,
        },
      ],
    },
    {
      key: guidGenerator(),
      path: 'system',
      label: 'Hệ thống',
      children: [
        {
          key: guidGenerator(),
          path: 'user-report',
          label: 'Báo cáo',
          description: 'Toàn bộ báo cáo hiển thị dưới đây',
          component: ListUserReport,
        },
        {
          key: guidGenerator(),
          path: 'address-list',
          label: 'Địa điểm bãi đỗ xe',
          description: 'Toàn bộ địa điểm bãi đỗ xe hiển thị dưới đây',
          component: ListAddressManager,
        },
      ],
    },
  ],
};
