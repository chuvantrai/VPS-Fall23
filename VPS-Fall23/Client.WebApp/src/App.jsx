import './App.css'
import { ConfigProvider, Spin } from 'antd';
import { App as VpsApp } from 'antd';
import { LoadingOutlined } from '@ant-design/icons';
import { useSelector } from 'react-redux';
const antIcon = <LoadingOutlined style={{ fontSize: 36 }} spin />;
function App() {
  const { isLoading } = useSelector(state => state.global);
  return (
    <ConfigProvider>
      <VpsApp className='app'>

        <Spin spinning={isLoading} indicator={antIcon}>
          {/**
            * 
            * TODO
            *  Đặt router page vô đây
            * */}
        </Spin>

      </VpsApp>
    </ConfigProvider>
  )
}

export default App
