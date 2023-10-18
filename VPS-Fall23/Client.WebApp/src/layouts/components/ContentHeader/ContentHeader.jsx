import { Breadcrumb } from "antd";

function ContentHeader({ title, desc }) {
  return (
    <div className="w-full bg-white py-[16px] px-[24px] mb-[20px]">
      <Breadcrumb>
        <Breadcrumb.Item>Home</Breadcrumb.Item>
        <Breadcrumb.Item>App</Breadcrumb.Item>
      </Breadcrumb>
      <div className="w-[808px] h-11 justify-start items-center gap-4 inline-flex mt-[8px] mb-[8px]">
        <div className="justify-start items-center gap-3 flex">
          <div className="text-black text-opacity-90 text-[22px] font-medium font-['Roboto'] leading-7">{title}</div>
        </div>
      </div>
      <div className="w-[1146px] text-zinc-800 text-[13px] font-normal font-['Roboto'] leading-[17.03px]">{desc}</div>
    </div>
  );
}

export default ContentHeader;