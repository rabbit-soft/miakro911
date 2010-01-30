using System;
using System.Collections.Generic;

namespace mia_conv
{
    partial class MDCreator
    {
        public void fillOptions()
        {
            debug("fill Options");
            MFParamForm p = mia.paramform;
            setOption("opt", "okrol", p.okrol.value());
            setOption("opt", "vudvor", p.vudvorenie.value());
            setOption("opt", "count1", p.pravka_1.value());
            setOption("opt", "count2", p.count_2.value());
            setOption("opt", "count3", p.count_3.value());
            setOption("opt", "bride", p.make_brides.value());
            setOption("opt", "preokrol", p.heater.value());
            setOption("opt", "combage", 3);
            setOption("opt", "malewait", 2);
            setOption("opt", "girlsout", 100);
            setOption("opt", "suckers", p.countsuckers.value());
            setOption("opt", "boysout", p.otsad_boys_mother.value());
            setOption("opt", "statefuck", p.vyazkamother.value());
            setOption("opt", "firstfuck", p.vyazkapervo.value());
            setOption("opt", "gentree", 10);
            setOption("opt", "confirmexit", 0);
            setOption("opt", "confirmkill", 1);
            setOption("opt", "vacc", p.vacc.value());
            setOption("opt", "nest", p.pervonest.value());
            setOption("opt", "childnest", p.mothernest.value());
            setOption("opt", "updatezoo", 1);
            //            setOption("opt", "kukunest", p.kukunest.value());
            //            setOption("opt", "otsad_boys_pervo", p.otsad_boys_pervo.value());
            /* //OPTIONS
            setOption("opt","kuk",p.kuk.value());
            setOption("opt","endkuku",p.endkuku.value());
            setOption("opt","killfemales",p.killfemales.value());
            setOption("opt","killbrides",p.killbrides.value());
            setOption("opt","killboys",p.killboys.value());
            setOption("opt","max_age_diff",p.max_age_diff.value());
            setOption("opt","automode",p.automode.value());
            setOption("opt","rescopies",p.rescopies.value());
             */
            setOption("opt","short_names",p.tab_abbr.value());
            setOption("opt","dbl_surname",p.double_sur.value());
            setOption("opt","heterosis",p.heterosis.value());
            setOption("opt","inbreeding",p.inbreeding.value());
            /*
            setOption("opt","report_full_addr",p.report_full_addr.value());
             */
            setOption("opt","sh_tier_t",p.show_tier_types.value());
            setOption("opt","sh_tier_s",p.show_area_types.value());
            /*
            setOption("opt","sluchka_filter",p.sluchka_filter.value());
            setOption("opt","zoo_bits",p.zoo_bits.value());
            setOption("opt","job_grouping",p.job_grouping.value());
            setOption("opt","name_show",p.name_show.value());
            setOption("opt","ignore_last_fuck",p.ignore_last_fuck.value());
            setOption("opt","partners_limit",p.partners_limit.value());
            setOption("opt","limit_value",p.limit_value.value());
            setOption("opt","sec_ignore",p.sec_ignore.value());
            setOption("opt","auto_kuk",p.auto_kuk.value());
            setOption("opt","jurta_sync",p.jurta_sync.value());
            setOption("opt","sell_mothers_with_babies",p.sell_mothers_with_babies.value());
            setOption("opt","imm_age_diff",p.imm_age_diff.value());
            setOption("opt","arctime",p.arctime.value());
            setOption("opt","lost_days",p.lost_days.value());
            setOption("opt","use_feed_spec",p.use_feed_spec.value());
            setOption("opt","auto_arc",p.auto_arc.value());
            setOption("opt","no_kuk",p.no_kuk.value());
            setOption("opt","no_gen_mix",p.no_gen_mix.value());
            setOption("opt","holost_punish",p.holost_punish.value());
            setOption("opt","imm_heater",p.imm_heater.value());
            setOption("opt","no_jurta_kuk",p.no_jurta_kuk.value());
            setOption("opt","shed_scale",p.shed_scale.value());
            setOption("opt","show_gen_tree",p.show_gen_tree.value());
            setOption("opt","show_young_gen_tree",p.show_young_gen_tree.value());
            setOption("opt","gen_tree_width",p.gen_tree_width.value());
            setOption("opt","young_gen_tree_width",p.young_gen_tree_width.value());
             */
            setOption("opt","sh_num",p.show_numbers.value());
            /*
            setOption("opt","averfeed",p.averfeed.value());
            setOption("opt","number_before_name",p.number_before_name.value());
             * */
            /* //  OTHEROPTS
            setOption("farm", "name", mia.thisfarm.value());
            setOption("farm", "id", mia.farmid.value());
            for (int i = 0; i < mia.wlist.weighters.Count; i++)
            {
                Weighter w=mia.wlist.weighters[i];
                setOption("weighter","start"+i.ToString(),(w.on.value()==0?0:w.start.value()));
                setOption("weighter","pos" + i.ToString(), (w.on.value() == 0 ? 0 : w.interval.value()));
            }
            setOption("weighter", "last", (mia.wlist.laston.value()==0?0:mia.wlist.lastpos.value()));
             * */

            for (int i = 0; i < p.jobs.Count; i++)
            {
                c.CommandText = "INSERT INTO jobs(j_name,j_short_name) VALUES('" + p.jobs[i].job.value() + "','" + p.jobs[i].name.value() + "');";
                c.ExecuteNonQuery();
            }
        }
    }
}
