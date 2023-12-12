import { Footer as AntdFooter } from 'antd/es/layout/layout';
import ModalReport from '@/components/modalDialogs/modalReport/index.js';
import { QuestionCircleOutlined } from '@ant-design/icons';

const contentBtnReport = (
  <>
    <QuestionCircleOutlined className={'text-[2.4rem] cursor-pointer'} />
  </>
);
const Footer = () => {
  return (
      <>
          <AntdFooter className={'py-[2.3rem]'}>
              <div className={'grid grid-cols-3 gap-4'}>
                  <div className={''}></div>
                  <div className={'text-center'}>VPS ©2023 Created by SEP490-G14</div>
                  <div className={'flex justify-end'}><ModalReport contentBtn={contentBtnReport} /></div>
              </div>
          </AntdFooter>
      </>
  );
};
export default Footer;
